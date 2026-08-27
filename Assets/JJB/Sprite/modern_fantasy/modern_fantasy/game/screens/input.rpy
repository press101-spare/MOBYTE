
## Input screen ################################################################
##
## This screen is used to display renpy.input. The prompt parameter is used to
## pass a text prompt in.
##
## This screen must create an input displayable with id "input" to accept the
## various input parameters.
##
## https://www.renpy.org/doc/html/screen_special.html#input

screen input(prompt):
    style_prefix "input"

    window:
        # This makes the background the same as the ADV dialogue box

        vbox:
            xanchor 0.0 ypos 60 spacing -5
            text prompt style "input_prompt"
            hbox:
                xoffset 100 spacing -15
                add "b_talk" yalign 0.5 yoffset 10
                input id "input" #xoffset 60

style input_prompt:
    xalign 0.0
    color light_accent
    ###Outlines to create shadow effect
    outlines [ (1, "#2b262609", 0, 0),  (2, "#00000007", 0, 0), (3, "#00000010", 0, 0), (4, "#00000007", 0, 0) ]


style input:
    xalign 0.0
    xmaximum 1116
    color white
    ###Outlines to create shadow effect
    outlines [ (1, "#2b262609", 0, 0),  (2, "#00000007", 0, 0), (3, "#00000010", 0, 0), (4, "#00000007", 0, 0) ]


