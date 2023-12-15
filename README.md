# SpellingBee
Solver for [New York Times Spelling Bee](https://www.nytimes.com/puzzles/spelling-bee) Game.

## Background
I like to play the New York Times Spelling Bee. It's a fun little challenge. While playing, I became curious about how hard it would be to write a program that would brute force solve the puzzle for me. I've come up with a simple solution below. Note that this solution is only as good as the dictionary we provide it (and the one here is a far cry from what the foils at the NYT use). But it was a fun exercise and I thought I'd share my results (and thought process) here.

# Rules

The New York Times Spelling Bee has the following rules / constraints:
- Words must have at least 4 letters
- Words can be a maximum of 19 letters.
- Words must be made up of the available letters.
- Words must contain the center letter.
- Only the characters a-z are allowed.

# Potential Solutions

# Solution 1 - Determine all possible permutations of words

It turns out there a lot of possible permutations.

For for letter words, we get the following:

Or at least that's how I _think_ it works. My combinatorial math is a bit rusty, so please forgive me.

4 letter words

n is the number of letters in the word.

```
(1 * 7 * 7 * 7) + (7 * 1 * 7 * 7) + (7 * 7 * 1 * 7) + (7 * 7 * 7 * 1) = 343 + 343 + 343 + 343 = 1,372
(7 ^ (n - 1)) * n 
(7 ^ (4 - 1)) * 4 
(7 ^ 3) * 4 
(343) * 4
1,372
```

To extrapolate, the series of numbers looks somethin like:

```
n=4
n=19

SUM{ (7 ^ (n - 1)) * n }
```

I'm lazy, so I wrote some code to do the math:

```c#
public static void Main()
{
    double total = 0;
    
    for(var n = 4; n < 19; n++)
    {
        total += (Math.Pow(7, n - 1) * n);
    }
    
    Console.WriteLine(total);
}
```

This gives us:

4,840,007,082,678,117

That's an awful lot of permutations. That would take a while. Not only would we have to come up with all of those permutations, we would have to iterate through them and do a lookup in our dictionary to see if it's a valid word. Ugh. That doesn't seem efficient.

# Solution 2 - Check all words in a dictionary to see if they match the available letters

It turns out that there are just over 50,000 words that match our requirements (4 - 19 letters, no special characters). But how to efficiently determine if a word matches the available letters?

Because we require that all letters be in the range a-z, that's 26 letters. A standard int in .NET has 32 bits, so we _could_ just create a bit mask of the letters in each word. Let's do that!

That makes the letter 'a' look like:

```
      zy xwvutsrq ponmlkji hgfedcba
00000000 00000000 00000000 00000001
```

The letter 'b' looks like: 

```
      zy xwvutsrq ponmlkji hgfedcba
00000000 00000000 00000000 00000010
```

And so on.

So let's take the puzzle 

```
 N D
R L I
 A O
```

The bit mask for these letters is:

```
      zy xwvutsrq ponmlkji hgfedcba
00000000 00000010 01101001 00001001
```

Let's take a candidate word that we know matches: land

```
      zy xwvutsrq ponmlkji hgfedcba
00000000 00000000 00101000 00001001
```

Because our lookup list is limited to only words with 4-19 letters consisting of the characters a-z, we just have to check to see if the candidate word has any letters that aren't part of our 7 letters. A way to do this is to invert that mask:

```
                                    zy xwvutsrq ponmlkji hgfedcba
allowed letters (ndarliao):   00000000 00000010 01101001 00001001
~allowed letters:             11111111 11111101 10010110 11110110            
```

Let's perform a bitwise and to determine if any "invalid" letters are present in the candidate word:

```
                                   zy xwvutsrq ponmlkji hgfedcba
~allowed letters (ndarliao): 11111111 11111101 10010110 11110110
candidate word   (land):     00000000 00000000 00101000 00001001
bitwise and:                 00000000 00000000 00000000 00000000            
```

The result is 0, so no invalid letters were found, so this is a solution to the puzzle!

A failed candidate word would like something like this:

```
                                   zy xwvutsrq ponmlkji hgfedcba
~allowed letters (ndarliao): 11111111 11111101 10010110 11110110
candidate word   (fool):     00000000 00000000 01001000 00100000
bitwise and:                 00000000 00000000 00000000 00100000
```

Oh snap. The result is non-zero. That means there is an invalid letter. Discard this result.

Okay, but what about the rule for needing to include the center letter? Glad you asked. We can do that by determining the bitmask for that required letter (in this case, it's 'L')

```
                                   zy xwvutsrq ponmlkji hgfedcba
required letter mask:        00000000 00000000 00001000 00000000
candidate word   (land):     00000000 00000000 00101000 00001001
bitwise and:                 00000000 00000000 00001000 00000000
```

This shows us that the letter 'l' is included in the candidate word! This is a valid match! Woot!

# Panagram

What about determining if we found a panagram? That's easy with some more mask magic!!!! Let's go back to the allowed letters, and re-use that mask:

```
                                    zy xwvutsrq ponmlkji hgfedcba
allowed letters (ndarliao):   00000000 00000010 01101001 00001001
panagram (radiolarian):       00000000 00000010 01101001 00001001
bitwise and:                  00000000 00000010 01101001 00001001
```

This shows that we have a panagram! 

# Execution Time

This is pretty fast by my standards:

```
  alan
  aland
  alar
  allod
  allodial
  alloo
  aloin
  ladin
  ladino
  laid
  lain
  lair
  laird
  lalo
  land
  landlord
  laniard
  lanioid
  lanolin
  lard
  lardon
  lardoon
  laroid
  liana
  liar
  liard
  nail
  nall
  nill
  nodal
  noll
  nonillion
  radial
  radialia
  radiolaria
* radiolarian
  radioli
  rail
  railroad
  ranal
  rial
  rill
  roial
  roil
  roll
  roral

  00:00:00.0005827
```
That's around 6 thousandths of a second using a single thread and no fancy tricks. Just meat and potatoes bitmask logic.

Would a ParallelForEach be faster? I didn't bother. I'm _guessing_ that the overhead of having perform synronization for multiple threads adding records to a collection and I don't think we'd gain much. See also - laziness.

# Projects

|Project|Description|
|---|---|
|SpellingBee.Engine|Core implementation of the word finding logic|
|SpellingBee.FileConverter|One time use console app that reads in a .csv file and outputs a specially formatted text file of words|
|SpellingBee.Host|Console Application that executes the word search.|

# File Format
To make loading of our dictionary fast, I've adopted the following file format for [dictionary.txt](src/SpellingBee.Host/dictionary.txt):

```
31829      # total number of entries in the dictionary
aaronic    # word[0]
155909     # mask[0]
aaronical  # word[1]
157957     # mask[1s]
...
```

Disclaimers:
- I haven't done the necessary profiling to determine if it's faster to just cacluate the word mask while loading the file. 
- I also haven't tried using a binary file format to see if that's any faster.

