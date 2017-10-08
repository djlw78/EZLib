﻿namespace EZLib
{
    public class EzLib
    {
        private readonly System _system = new System();

        internal string MyUsername { get; }
        internal string MyIpAddress { get; }
        internal string MyLicenseKey { get; }
        internal string MyHardwareId { get; }

        public bool Initialize(string programId)
        {
            if (programId == string.Empty)
                return false;
            _system.InitializeProgram(programId);
            return _system.IsInitialized;
        }


        #region User Information

        public string GetUsername()
        {
            return MyUsername;
        }

        public string GetIpAddress()
        {
            return MyIpAddress;
        }

        public string GetLicenseKey()
        {
            return MyLicenseKey;
        }

        public string GetHardwareId()
        {
            return MyHardwareId;
        }

        #endregion
    }
}