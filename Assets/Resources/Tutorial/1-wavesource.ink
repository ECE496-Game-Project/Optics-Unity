INCLUDE global.ink

{ knowWaveSource:
- false: ->intro
- true: ->repeat
}

=== intro ===
How are doing today, sir!
+ [I am doing great, thanks!]
    I am glad to hear that.
+ [Not bad, I guess.]
    Oh, I wish the rest of your day would be better.
- Sir, I know you are wondering who I am.
+ [Actually, yes.]
+ [Since you ask, yes]
- Thank you, sir. May I introduce myself. My name is Wave Source. #title:Wave Source #speaker:Source
To be simple, my job is to produce light.
~ knowWaveSource = true
Please feel free to ask me anything you want to know, sir.
->choices

=== choices ===
+ [I want to know more about what you have produced!]->wave
+ {isWaveTutFinish} [Let's do a quiz!]->quiz
+ [No other things I want to know.]->end

=== wave ===
Of course, sir. Let me introduce the wave light to you. #title:Wave Knowledge 
The Optical Electic Wave contains both Temporal and Spatial frequency, meaning that the magnitue of wave will change w.r.t. both time and space.#image:Tutorial/page2

This can also be seen from the harmonic wave equaction as it contains k and w. #image:Tutorial/page3
~ isWaveTutFinish = true
* [CONTINUE]->repeat

=== quiz ===
What's two component does harmonic wave contains? #title:Wave Quiz
    + [It contains temporal and spatial frequency]
        Your answer is correct
        ** [CONTINUE]-> repeat
    + [It contains animation and physics]
        Your answer is incorrect->quiz

=== repeat ===
Sir, please feel free to ask me anything. #title:Wave Source #speaker:Source
->choices

=== end ===
You could invoke me again by clicking me again. May you have a good learning experience!
* [LEAVE]->DONE