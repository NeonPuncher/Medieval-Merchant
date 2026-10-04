
VAR DidReturn = false


=== monsterhunter0 === 
This is the first line of dialogue. #portrait:0

//Test comment
This is the second line of dialogue.

Do you have anything to traide
* [Trade]
    Very nice! #portrait:1
    -> END
* [Later]
    Alright I'll come back later :)
    -> END
* [Refuse]
    What a shame...  #portrait:2
    -> END
    
=== MonsterHunterReturn ===
{DidReturn:
    Hello I've come back, hopefully you have my weapon ready!
  - else:
    Your weapon was ass, it broke >:( I want a better one!
}
-> DONE

=== monsterhunter1 ===

The Sword broke during battle

I demand a refund

-> END

=== monsterhunter2 ===

The Sword was awesome :D 

Here is a reward

-> END