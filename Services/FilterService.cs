using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Services
{
    public class FilterService
    {

        private CancellationTokenSource _debounceCts;

        private readonly Func<Task> FilterCB;


        public FilterService(Func<Task> filterCB)
        {
            FilterCB = filterCB;
        }

        public async void DebouncePropertyChanged([CallerMemberName] string propertyName = null)
        {
            _debounceCts?.Cancel(); // Cancel previous pending task
            _debounceCts = new CancellationTokenSource();
            var token = _debounceCts.Token;

            try
            {
                await Task.Delay(600, token);

                if (!token.IsCancellationRequested)
                {
                    FilterCB?.Invoke();
                }
            }
            catch (TaskCanceledException)
            {
                // Ignore task cancellation
            }
        }
    }
}
