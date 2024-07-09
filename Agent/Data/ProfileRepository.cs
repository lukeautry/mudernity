using Agent.Types.Models;
using Agent.Types.Responses;

namespace Agent.Data
{
    public class ProfileRepository(DatastoreService store)
    {
        public ProfileList GetProfileList()
        {
            return new ProfileList
            {
                Profiles = GetProfiles()
            };
        }

        public List<Profile> GetProfiles()
        {
            return store.Query(store => store.Profiles);
        }

        public Profile? GetProfile(string id)
        {
            return GetProfiles().FirstOrDefault(profile => profile.Id == id);
        }

        public void AddProfile(Profile profile)
        {
            store.Mutate(store =>
            {
                return store with
                {
                    Profiles = [.. store.Profiles, profile]
                };
            });
        }

        public void DeleteProfile(string profileId)
        {
            store.Mutate(store =>
            {
                return store with
                {
                    Profiles = store.Profiles.Where(profile => profile.Id != profileId).ToList()
                };
            });
        }

        public void UpdateProfile(Profile profile)
        {
            store.Mutate(store =>
            {
                return store with
                {
                    Profiles = store.Profiles.Select(p =>
                    {
                        if (p.Id == profile.Id)
                        {
                            return profile;
                        }

                        return p;
                    }).ToList()
                };
            });
        }
    }
}