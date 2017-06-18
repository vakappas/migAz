using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.AWS
{
    public class EbsVolume
    {
        private string _volumeId, _volumeName;
        public EbsVolume(string volumeId)
        {
            _volumeId = volumeId;
        }

        public EbsVolume(string volumeId, string volumeName)
        {
            _volumeId = volumeId;
            _volumeName = volumeName;
        }
        public string VolumeId {
            get
            {
                return _volumeId;
            }
            private set
            {
                _volumeId = value;
            }
        }
        public string VolumeName {
            get
            {
                return _volumeName;
            }
            private set
            {
                _volumeName = value;
            }
        }
    }
}
