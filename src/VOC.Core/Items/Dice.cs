using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Items
{
    public class Dice : IDice
    {
        private readonly IEnumerable<IDie> dice;

        /// <summary>
        /// Creates a dice instance with 2 dice
        /// </summary>
        public Dice()
        {
            var random = new Random();
            dice = new IDie[] { new Die(random), new Die(random) };
            Roll();
        }

        public Dice(IEnumerable<IDie> dice)
        {
            if (dice == null)
                throw new ArgumentNullException(nameof(dice));
            if (!dice.Any())
                throw new ArgumentException("Need at least 1 die");
            this.dice = dice;
            Roll();
        }

        public DiceRoll Current { get; private set; }
        public void Roll()
        {
            Current = new DiceRoll(dice.Select(d => d.Throw()));
        }
    }
}
