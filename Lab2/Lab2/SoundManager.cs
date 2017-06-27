using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToA
{
    public class SoundManager
    {
        Dictionary<string, SoundEffect> sounds;
        Dictionary<string, Song> songs;

        public Dictionary<string, SoundEffect> Sounds
        {
            get
            {
                return sounds;
            }
        }

        public Dictionary<string, Song> Songs
        {
            get
            {
                return songs;
            }
        }

        public SoundManager()
        {
            sounds = new Dictionary<string, SoundEffect>();
            songs = new Dictionary<string, Song>();
        }
    }
}
