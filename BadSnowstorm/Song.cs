using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BadSnowstorm
{
    public class Song
    {
        private readonly Tempo _tempo;
        private readonly Note[] _notes;

        public Song(Tempo tempo, params Note[] notes)
        {
            _tempo = tempo;
            _notes = notes;
        }

        public Song(Tempo tempo, IEnumerable<Note> notes)
            : this(tempo, notes.ToArray())
        {
        }

        public void Play(IConsole console, bool cancelOnEscape = false)
        {
            bool isCancelled = false;

            Action playNotes = () =>
            {
                foreach (var note in _notes)
                {
                    var durationMilliseconds = _tempo.GetDurationMilliseconds(note.NoteDuration);

                    if (note.NoteTone == Tone.Rest)
                    {
                        Thread.Sleep(durationMilliseconds);
                    }
                    else
                    {
                        console.Beep((int)note.NoteTone, durationMilliseconds);
                    }

                    if (isCancelled)
                    {
                        return;
                    }
                }
            };

            if (cancelOnEscape)
            {
                const char escape = (char)27;

                var playNotesTask = Task.Factory.StartNew(
                    () =>
                    {
                        playNotes();
                        if (!isCancelled)
                        {
                            console.SendChar(escape);
                        }
                    });

                var readKeyTask = Task.Factory.StartNew(
                    () =>
                    {
                        while (console.ReadKey() != escape)
                        {
                        }
                        isCancelled = true;
                    });
                
                Task.WaitAll(playNotesTask, readKeyTask);
            }
            else
            {
                playNotes();
            }
        }
    }
}