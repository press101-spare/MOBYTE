# The script of the game goes in this file.

# Declare characters used by this game. The color argument colorizes the
# name of the character.

define e = Character("Eileen")
define s = Character("Sylvie", image="sylvie", kind=bubble)





init python:

    ###Define the name of the savefile at the beginning of the game here
    save_name = "Prologue"


###Cursor
define config.mouse = {"default":[ ("gui/cursor.png", 1, 1) ] }


# The game starts here.

label start:

    # Show a background. This uses a placeholder by default, but you can
    # add a file (named either "bg room.png" or "bg room.jpg") to the
    # images directory to show it.

    scene bg room with fade


    # This shows a character sprite. A placeholder is used, but you can
    # replace it by adding a file named "eileen happy.png" to the images
    # directory.

    show eileen happy

    # These display lines of dialogue.

    e "You've created a new Ren'Py game."

    e "Once you add a story, pictures, and music, you can release it to the world!"
    # This ends the game.



    label test:
        menu:

            "What would you like to test?"
            

            "Show ADV text"(badge= "b_talk"):
                jump adv

            
            "Show bubble text":
                jump bubblet


            "Change savefile name":
                $ save_name = "Chapter 1"
                "Change made."

                jump test

            "More" (badge= "b_move"):
                menu:
                    
                    "Additional possibilities (it's best to add some dialogue like this for choices or turn off the quick menu before them. Otherwise the quick menu will just float.)"
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras aliquam massa et ipsum mattis eleifend. Long choice." (badge= "b_talk"):
                        jump test

                    "Input screen":
                        $ answer = renpy.input("What were you doing yesterday 4 p.m. at the City Hotel?", default="Input text here", length=50).strip().lower() ####about 50 characters is the max when it comes to the input screen so I recommend putting that length here
                        jump test

                    "Notification":
                        $ renpy.notify("Hello there!")
                        jump test

            "End Game" (badge= "b_move"):
                pass




    return




label adv:
    e "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras aliquam massa et ipsum mattis eleifend. Mauris non nunc et diam aliquet pharetra sed et quam."

    e "Proin id urna dolor. Pellentesque tempor tortor sapien, at consequat libero ullamcorper a. Curabitur in lorem libero. Integer in suscipit augue, sed consectetur nunc. Pellentesque id odio eu libero blandit facilisis vitae rhoncus lacus."

    "Quisque eu purus erat. Sed gravida felis id congue laoreet. Integer ac efficitur nulla. Quisque tempor eros at magna aliquam, sed tempor quam iaculis. Integer tempor lacus rutrum laoreet placerat."

    e "Cras sollicitudin magna id nisl dignissim, ut condimentum nunc imperdiet. Nulla congue imperdiet commodo. Phasellus fermentum nec risus vel finibus."

    "Sed elit tellus, consequat a auctor ac, luctus sit amet tellus. Curabitur laoreet justo malesuada eros scelerisque, ac condimentum nulla molestie. Sed vehicula nulla nulla, ac eleifend dui auctor ac. "

    jump test


label bubblet:
    s "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras aliquam massa et ipsum mattis eleifend. Mauris non nunc et diam aliquet pharetra sed et quam."

    s "Proin id urna dolor. Pellentesque tempor tortor sapien, at consequat libero ullamcorper a. Curabitur in lorem libero. Integer in suscipit augue, sed consectetur nunc. Pellentesque id odio eu libero blandit facilisis vitae rhoncus lacus."

    s"Quisque eu purus erat. Sed gravida felis id congue laoreet. Integer ac efficitur nulla. Quisque tempor eros at magna aliquam, sed tempor quam iaculis. Integer tempor lacus rutrum laoreet placerat."

    s "Cras sollicitudin magna id nisl dignissim, ut condimentum nunc imperdiet. Nulla congue imperdiet commodo. Phasellus fermentum nec risus vel finibus."

    s"Sed elit tellus, consequat a auctor ac, luctus sit amet tellus. Curabitur laoreet justo malesuada eros scelerisque, ac condimentum nulla molestie. Sed vehicula nulla nulla, ac eleifend dui auctor ac. "

    jump test




