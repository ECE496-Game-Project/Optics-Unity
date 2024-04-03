-> wavesource

=== wavesource ===
Hello, there!
I know you are wondering what I am.
My name is Wave Source. #title:Wave Source #speaker:Source #portrait:WAVE_SOURCE
I produce light.
->choices

=== choices ===
What do you want?
+ [Let's do a quiz!]->quiz
+ [Learn about optics!]->wave
+ [No other things I want to know.]->end

=== quiz ===
VAR answer = 0
What's the answer?
    + [Answer is 1]
        ~ answer = 1
    + [Answer is 2]
        ~ answer = 2
- 
{ answer:
- 1: 
    Your answer is {answer}, it is correct
    -> repeat
- else: 
    Your answer is {answer}, it is incorrect
    ->quiz
}
-> repeat

=== wave ===
#image:Tutorial/page2
#image:Tutorial/page3
->repeat

=== repeat ===
Do you have any other things want to know? #title:System 
->choices

=== end ===
You could invoke me again by clicking the button with logo at anytime. May you have a good learning experience!
* [LEAVE]->DONE