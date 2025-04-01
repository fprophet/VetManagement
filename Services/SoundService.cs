using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Services
{
    public class SoundService
    {
        [DllImport("kernel32.dll")]
        public static extern bool Beep(int frequency, int duration);

        public static void PlayTone()
        {
            Beep(1000, 500); // Frequency in Hz, Duration in ms
        }

        public static async Task PlayNotificationSound()
        {
            Beep(600, 200);
            await Task.Delay(25);
            Beep(750, 200);
            await Task.Delay(25);
            Beep(600, 200);
        }
    }
}
