using System;
using System.Collections.Concurrent;
using System.Threading;
using NUnit.Framework;

namespace Test
{
    /// <summary>
    /// Class helping developers to write unit tests where the test has to wait for an event.
    /// </summary>
    public class EventAsserter
    {
        private const int DefaultMillisecondsTimeout = 50;

        private readonly ConcurrentQueue<EventArgs> payloadQueue;
        private readonly AutoResetEvent autoResetEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAsserter"/> class.
        /// </summary>
        public EventAsserter()
        {
            payloadQueue = new ConcurrentQueue<EventArgs>();
            autoResetEvent = new AutoResetEvent(false);
        }

        /// <summary>
        /// Gets the number of events received.
        /// </summary>
        public int Count
        {
            get { return payloadQueue.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether no more event payloads exist.
        /// </summary>
        public bool IsEmpty
        {
            get { return payloadQueue.IsEmpty; }
        }

        /// <summary>
        /// Removes and returns the payload of a received event.
        /// </summary>
        public T Dequeue<T>() where T : EventArgs
        {
            EventArgs result;
            if (payloadQueue.TryDequeue(out result))
            {
                return (T)result;
            }

            throw new InvalidOperationException("No more events have been received.");
        }

        /// <summary>
        /// Callback for the event to monitor.
        /// </summary>
        public void Handler(object sender, EventArgs e)
        {
            Assert.IsNotNull(e);

            payloadQueue.Enqueue(e);
            autoResetEvent.Set();
        }

        /// <summary>
        /// Wait for event to be received. If no event is received, an exception is thrown
        /// </summary>
        /// <param name="millisecondsTimeout">
        /// The timeout in milliseconds to wait for an event.
        /// </param>
        /// <returns>The time this class stopped waiting for events.</returns>
        public DateTime AssertEventReceived(int millisecondsTimeout = DefaultMillisecondsTimeout)
        {
            Assert.IsTrue(millisecondsTimeout > 0);
            Assert.IsTrue(autoResetEvent.WaitOne(millisecondsTimeout), "The wait timed out, when an event was expected.");

            return DateTime.Now;
        }

        /// <summary>
        /// Wait for event and expect a timeout. If an event is received, an exception is thrown
        /// </summary>
        /// <param name="millisecondsTimeout">
        /// The timeout in milliseconds to wait for an event.
        /// </param>
        /// <returns>The time this class stopped waiting for events.</returns>
        public DateTime AssertNoEventReceived(int millisecondsTimeout = DefaultMillisecondsTimeout)
        {
            Assert.IsTrue(millisecondsTimeout > 0);
            Assert.IsFalse(autoResetEvent.WaitOne(millisecondsTimeout), "An unexpected event arrived, when a timeout is expected.");

            return DateTime.Now;
        }
    }
}