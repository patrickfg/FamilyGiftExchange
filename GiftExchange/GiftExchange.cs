using GiftExchange.Interfaces;
using Serilog;

namespace GiftExchange;

public class GiftExchange : IGiftExchange {
	private readonly IParticipant[] _participants;

	private readonly Random _random = new Random();

	public GiftExchange(IParticipantSource participantSource) {
		_participants = participantSource.GetParticipants();
	}

	public IParticipant[] Participants => _participants.ToArray();

	public void RandomizeRecipients() {
		Log.Debug("Starting gift recipient randomization");

		ValidateParticipants();

		int maxAttempts = _participants.Length - 1;

		for (int attempt = 0; attempt < maxAttempts; attempt++) {
			Log.Debug("Randomization attempt {Attempt} of {Max}", attempt + 1, maxAttempts);
			ResetAssignments();

			if (TryRandomizeAssignments()) {
				Log.Information("Successfully randomized gift recipients after {Attempt} attempts", attempt + 1);
				return;
			}
		}
	}

	private bool TryRandomizeAssignments() {
		Log.Debug("Attempting to randomize gift assignments");

		var participants = _participants.OrderBy(p => p.Exclusions.Length).ToArray();
		List<IParticipant> alreadyAssignedParticippants = new();

		foreach (var participant in participants) {
			Log.Debug("Assigning recipient for participant {Participant}", participant.FullName);
			Log.Debug("{PreviousAssignments} participants have already been assigned recipients", alreadyAssignedParticippants.Count);
			Log.Debug("Participant {Participant} has {ExclusionCount} exclusions", participant.FullName, participant.Exclusions.Length);

			int[] candidates = participants
				.Where(p => p.Id != participant.Id && !participant.Exclusions.Contains(p) && !alreadyAssignedParticippants.Contains(p))
				.Select(p => p.Id)
				.ToArray();

			if (!candidates.Any()) {
				Log.Debug("No valid candidates found for {Participant}", participant.FullName);
				return false;
			}

			Log.Debug("Participant {Participant} has {CandidateCount} valid candidates", participant.FullName, candidates.Length);

			int selectedCandidateId = candidates[_random.Next(candidates.Length)];
			var selectedCandidate = participants.Single(p => p.Id == selectedCandidateId);
			Log.Debug("Participant {Participant} assigned to gift to {Recipient}", participant.FullName, selectedCandidate.FullName);
			participant.AssignedRecipient = selectedCandidate;
			alreadyAssignedParticippants.Add(selectedCandidate);
		}

		return true;
	}

	private void ValidateParticipants() {
		Log.Debug("Validating participants for gift exchange");
		if (_participants.Length < 2) {
			Log.Error("Not enough participants for gift exchange. At least 2 are required.");
			throw new InvalidOperationException("At least two participants are required for the gift exchange.");
		}

		var distictIds = _participants.Select(p => p.Id).Distinct().Count();
		if (distictIds != _participants.Length) {
			Log.Error("Duplicate participant IDs found in the gift exchange.");
			throw new InvalidOperationException("Participant IDs must be unique.");
		}

		int maxExclusions = _participants.Length - 2;
		foreach (var participant in _participants) {
			if (participant.Exclusions.Length > maxExclusions) {
				Log.Error("Participant {Name} has to many exclusions.", participant.FullName);
				throw new InvalidOperationException($"Participant has to many exclusions.");
			}
		}
	}

	private void ResetAssignments() {
		Log.Debug("Resetting all participant assignments");
		_participants.All(p => p.AssignedRecipient == null);
	}
}