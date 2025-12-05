using System;

namespace SmartNutriTracker.Front.Services
{
    public class UserStateService
    {
        private string? _username;
        private string? _rol;
        private int? _userId;

        public event Action? OnChange;

        public string? Username
        {
            get => _username;
            set
            {
                _username = value;
                NotifyStateChanged();
            }
        }

        public string? Rol
        {
            get => _rol;
            set
            {
                _rol = value;
                NotifyStateChanged();
            }
        }

        public int? UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                NotifyStateChanged();
            }
        }

        public bool IsAuthenticated => !string.IsNullOrEmpty(_username);

        public void Clear()
        {
            _username = null;
            _rol = null;
            _userId = null;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
