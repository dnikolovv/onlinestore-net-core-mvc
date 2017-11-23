using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineStore.Infrastructure.Authorization
{
    public static class AuthorizationSectionsContainer
    {
        public static IReadOnlyCollection<AuthorizationSection> Sections
        {
            get
            {
                if (_sections == null || _sections.Count == 0)
                {
                    throw new InvalidOperationException(
                        "There are no sections initialized. " +
                        "Please call AuthorizationSectionsContanier.Initialize() " +
                        "or AddDynamicAuthorization() in your Startup.cs");
                }

                return _sections;
            }
        }

        private static IReadOnlyCollection<AuthorizationSection> _sections;

        public static void Initialize(params AuthorizationSection[] sections)
        {
            if (_sections == null || _sections.Count == 0)
            {
                _sections = sections
                        .ToList()
                        .AsReadOnly();
            }
            else
            {
                throw new InvalidOperationException("AuthorizationSectionsContainer has already been initialized.");
            }
        }
    }
}
