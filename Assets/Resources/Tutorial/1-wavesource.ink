INCLUDE global.ink

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
Do you want to know anything from me, sir?
->choices

=== choices ===
+ [I want to know more about what you have produced!]->wave
+ {isWaveTutFinish} [Let's do a quiz!]->quiz
+ [No other things I want to know.]->end

=== wave ===
Of course, sir. Let me introduce the wave light to you. #title:Wave Knowledge #image:Tutorial/page2 #image:Tutorial/page3
~ isWaveTutFinish = true
* [CONTINUE]->repeat

=== quiz ===
What's a harmonic plane wave? #title:Wave Quiz
    + [It is a harmonic wave]
        Your answer is correct
        ** [CONTINUE]-> repeat
    + [It is not a harmonic wave]
        Your answer is incorrect->quiz

=== repeat ===
Do you have any other things want to know? #title:Wave Source 
->choices

=== end ===
You could invoke me again by clicking me again. May you have a good learning experience!
* [LEAVE]->DONE