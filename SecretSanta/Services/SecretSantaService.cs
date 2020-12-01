using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecretSanta.Data;

namespace SecretSanta.Services
{
    public class SecretSantaService
    {
        public SecretSantaService()
        {

        }

        public async Task<bool> CheckAndAddParticipant(Participant participant, List<Participant> participants)
        {
            var result = participants.Where(p =>
                                            p.Email.Equals(participant.Email))
                                            .FirstOrDefault();

            if (result == null)
            {
                // Participant does not exist.
                // Add
                participant.Id = participants.Count + 1;
                participants.Add(participant);
                return await Task.FromResult(true);
            }
            else
            {
                // Participant already exists.
                return await Task.FromResult(false);
            }
        }

        public async Task<List<Participant>> GenerateSecretSanta(List<Participant> participants)
        {
            var maxSeed = participants.Count() + 1;

            List<int> secretSantaIds = new List<int>();

            foreach (var p in participants)
            {
                var randomNumber = GenerateRandomNumber(p.Id, maxSeed, secretSantaIds);

                var ss = participants.Where(p => p.Id.Equals(randomNumber)).FirstOrDefault();

                if(ss==null)
                {
                    throw new Exception("Generating Secret Santa failed!!!");
                }

                p.SecretSanta.Id = ss.Id;
                p.SecretSanta.Name = ss.Name;
                p.SecretSanta.Email = ss.Email;
            }

            return await Task.FromResult(participants);
        }

        private int GenerateRandomNumber(int participantId, int maxSeed, List<int> secretSantaIds)
        {
            int randomNumber = 0;

            do
            {
                randomNumber = new Random().Next(1, maxSeed);
            } while (AnotherCheckRequired(randomNumber, participantId, secretSantaIds));

            secretSantaIds.Add(randomNumber);

            return randomNumber;
        }

        private bool AnotherCheckRequired(int randomNumber, int participantId, List<int> secretSantaIds)
        {
            if (randomNumber == participantId || secretSantaIds.Contains(randomNumber))
            {
                return true;
            }

            return false;
        }
    }
}
