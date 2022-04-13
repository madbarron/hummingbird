using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Animations;

public class RandomQuote: MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<string> quotes = new List<string>();
        quotes.Add("“The daily hummingbird assaults existence with improbability.”\n– Ursula K. Le Guin");
        quotes.Add("“Moment is a flower. Mindfulness is sipping the nectar of that flower.”\n– Amit Ray");
        quotes.Add("“If you take a flower in your hand and really look at it, it's your world for a moment.”\n– Georgia O'Keefe");
        quotes.Add("“Every flower is a soul blossoming in nature.”\n– Gérard de Nerval");
        quotes.Add("“One day we'll all be flowers again.”\n– Merlin Schönfisch");
        quotes.Add("“The greatest weariness comes from work not done.”\n– Eric Hoffer");
        quotes.Add("“Some of my old memories feel trapped in amber in my brain, lucid and burning,\n" +
            "while others are like the wing beat of a hummingbird, an intangible, ephemeral blur.”\n– Mira Bartok");

        int index = (int)(quotes.Count * Random.value);
        TextMeshProUGUI textbox = GetComponent<TextMeshProUGUI>();
        textbox.text = quotes[index];
    }
}
