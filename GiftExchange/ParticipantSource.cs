using GiftExchange.Interfaces;
using Serilog;

namespace GiftExchange;

public class ParticipantSource : IParticipantSource {
	private List<IParticipant> _participants = new List<IParticipant>();
	private bool _allSpousesExcluded = false;

	public ParticipantSource() { }

	public ParticipantSource(IEnumerable<IParticipant> participants) {
		if (participants is null) {
			throw new ArgumentNullException(nameof(participants), "Participants collection cannot be null.");
		}
		if (!participants.Any()) {
			throw new ArgumentException("Participants collection cannot be empty.", nameof(participants));
		}
		
		ValidateParticipants(participants);

		_participants = participants.ToList();
	}

	public IParticipant[] GetParticipants() {
		if (!_allSpousesExcluded) {
			ExcludeSpouses();
		}
		return _participants.ToArray();
	}

	public void AddParticipant(IParticipant participant) {
		if (participant is null) {
			throw new ArgumentNullException(nameof(participant), "Participant cannot be null.");
		}

		var testParticipants = _participants.ToList();
		testParticipants.Add(participant);
		ValidateParticipants(testParticipants);

		_participants.Add(participant);
		_allSpousesExcluded = false;
	}

	public void LoadParticipants(string fileName) {
		Log.Debug("Loading participants from file: {FileName}", fileName);

		if (!File.Exists(fileName)) {
			Log.Error("File not found: {FileName}", fileName);
			return;
		}

		try {
			var json = File.ReadAllText(fileName);
			var participants = System.Text.Json.JsonSerializer.Deserialize<ParticipantRecord[]>(json);

			if (participants is null || !participants.Any()) {
				Log.Warning("No participants found in file: {FileName}", fileName);
				return;
			}

			List<IParticipant> loadedParticipants = new List<IParticipant>();
			foreach (var record in participants) {
				loadedParticipants.Add(new Participant(record));
			}
			ValidateParticipants(loadedParticipants);
			_participants = loadedParticipants;
		}
		catch (Exception ex) {
			Log.Error(ex, "Error loading participants from file: {FileName}", fileName);
		}
		ExcludeSpouses();
	}

	public void RemoveParticipant(IParticipant participant) {
		if (participant is null) {
			throw new ArgumentNullException(nameof(participant), "Participant cannot be null.");
		}
		if (!_participants.Contains(participant)) {
			throw new ArgumentException("Participant not found in the source.", nameof(participant));
		}
		_participants.Remove(participant);
	}

	public void SaveParticipants(string fileName) {
		Log.Debug("Saving participants to file: {FileName}", fileName);
		var options = new System.Text.Json.JsonSerializerOptions {
			WriteIndented = true
		};
		var participantRecords = _participants.Select(p => p.ToRecord()).ToArray();
		var json = System.Text.Json.JsonSerializer.Serialize(participantRecords, options);
		File.WriteAllText(fileName, json);
	}

	private void ExcludeSpouses() {
		var spouseDictionary = _participants.ToDictionary(p => p.FullName, p => p);

		foreach (var participant in _participants) {
			
			if (string.IsNullOrEmpty(participant.SpouseName)) {
				continue;
			}

			if (spouseDictionary.ContainsKey(participant.SpouseName)) {

				var spouse = spouseDictionary[participant.SpouseName];

				if (!participant.Exclusions.Contains(spouse)) {
					participant.AddExclusion(spouse);
				}

				if (!spouse.Exclusions.Contains(participant)) {
					spouse.AddExclusion(participant);
				}
			}
			else {
				Log.Warning("Spouse '{SpouseName}' for participant '{ParticipantName}' not found in participant list.", participant.SpouseName, participant.FullName);
			}
		}
		_allSpousesExcluded = true;
	}

	private static void ValidateParticipants(IEnumerable<IParticipant> participants) {
		var participantList = participants.ToList();

		var participantCount = participantList.Count;
		var idCount = participantList.Select(p => p.Id).Distinct().Count();
		var nameCount = participantList.Select(p => p.FullName).Distinct().Count();
		if (participantCount != idCount) {
			throw new ArgumentException("Duplicate participant IDs found.");
		}
		if (participantCount != nameCount) {
			throw new ArgumentException("Duplicate participant names found.");
		}
	}
}