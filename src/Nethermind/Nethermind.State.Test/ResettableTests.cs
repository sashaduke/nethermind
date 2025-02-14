// SPDX-FileCopyrightText: 2022 Demerzel Solutions Limited
// SPDX-License-Identifier: LGPL-3.0-only

using Nethermind.Core.Resettables;
using NUnit.Framework;

namespace Nethermind.Store.Test
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class ResettableTests
    {
        [Test]
        public void Can_resize_up()
        {
            int capacity = Resettable.StartCapacity;
            int currentPosition = Resettable.StartCapacity;
            int[] array = new int[Resettable.StartCapacity];
            Resettable<int>.IncrementPosition(ref array, ref capacity, ref currentPosition);

            Assert.AreEqual(Resettable.StartCapacity * Resettable.ResetRatio, array.Length);
        }

        [Test]
        public void Resets_on_position_reset()
        {
            int capacity = Resettable.StartCapacity;
            int currentPosition = Resettable.StartCapacity;
            int[] array = new int[Resettable.StartCapacity];
            array[0] = 30;

            Resettable<int>.Reset(ref array, ref capacity, ref currentPosition);

            Assert.AreEqual(Resettable.StartCapacity, array.Length);
            Assert.AreEqual(0, array[0]);
        }

        [Test]
        public void Can_resize_down()
        {
            int capacity = Resettable.StartCapacity;
            int currentPosition = capacity;
            int[] array = new int[Resettable.StartCapacity];
            Resettable<int>.IncrementPosition(ref array, ref capacity, ref currentPosition);

            Assert.AreEqual(Resettable.StartCapacity * Resettable.ResetRatio, array.Length);

            currentPosition -= 2;
            Resettable<int>.Reset(ref array, ref capacity, ref currentPosition, Resettable.StartCapacity);

            Assert.AreEqual(Resettable.StartCapacity, array.Length);
        }

        [Test]
        public void Does_not_resize_when_capacity_was_in_use()
        {
            int capacity = Resettable.StartCapacity;
            int currentPosition = Resettable.StartCapacity;
            int[] array = new int[Resettable.StartCapacity];
            Resettable<int>.IncrementPosition(ref array, ref capacity, ref currentPosition);

            Assert.AreEqual(Resettable.StartCapacity * Resettable.ResetRatio, array.Length);

            Resettable<int>.Reset(ref array, ref capacity, ref currentPosition, Resettable.StartCapacity);

            Assert.AreEqual(Resettable.StartCapacity * Resettable.ResetRatio, array.Length);
        }

        [Test]
        public void Delays_downsizing()
        {
            int capacity = Resettable.StartCapacity;
            int currentPosition = Resettable.StartCapacity * Resettable.ResetRatio;
            int[] array = new int[Resettable.StartCapacity];

            Resettable<int>.IncrementPosition(ref array, ref capacity, ref currentPosition);
            Assert.AreEqual(Resettable.StartCapacity * Resettable.ResetRatio * Resettable.ResetRatio, array.Length);

            Resettable<int>.Reset(ref array, ref capacity, ref currentPosition);
            Assert.AreEqual(Resettable.StartCapacity * Resettable.ResetRatio * Resettable.ResetRatio, array.Length);

            Resettable<int>.Reset(ref array, ref capacity, ref currentPosition);
            Assert.AreEqual(Resettable.StartCapacity * Resettable.ResetRatio, array.Length);
        }

        [Test]
        public void Copies_values_on_resize_up()
        {
            int capacity = Resettable.StartCapacity;
            int currentPosition = capacity;
            int[] array = new int[Resettable.StartCapacity];
            array[0] = 30;
            Resettable<int>.IncrementPosition(ref array, ref capacity, ref currentPosition);

            Assert.AreEqual(30, array[0]);
        }
    }
}
