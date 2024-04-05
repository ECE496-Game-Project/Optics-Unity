INCLUDE global.ink

{ knowWaveplate:
- false: ->intro1
- true: ->repeat
}

=== intro1 ===
... ...
+ [Hello?] ->intro2
+ [... ...] ->intro1

=== intro2 ===
Oh, hello. Sorry, I didn't see you.
+ [I know you, you are a polarizer]
    Bro, that is my brother. I am the waveplate.
+ [Wait, have we met before?]
    No, not at all. You must have met my brother. My name is waveplate. 
- In general, I could filt polarization light. #title:Waveplate #speaker:Waveplate
~ knowWaveplate = true
Why do you come here?
->choices

=== choices ===
+ [Could you tell me more about yourself?]->polarizer
+ {isWaveplateTutFinish} [Let's do a quiz!]->quiz
+ [No other things I want to know.]->end

=== polarizer ===
To be continued... #title:Waveplate Knowledge
~ isWaveplateTutFinish = true
* [CONTINUE]->repeat

=== quiz ===
To be continued... #title:Waveplate Quiz
* [CONTINUE]->repeat

=== repeat ===
Do you have any other things to ask? #title:Waveplate #speaker:Waveplate
->choices

=== end ===
You could invoke me again by clicking any waveplates. May you have a good learning experience!
* [LEAVE]->DONE