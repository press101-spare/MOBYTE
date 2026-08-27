
## Choice screen ###############################################################
##
## This screen is used to display the in-game choices presented by the menu
## statement. The one parameter, items, is a list of objects, each with caption
## and action fields.
##
## https://www.renpy.org/doc/html/screen_special.html#choice


image b_talk:
    offset(-23, -8)
    "gui/button/selector_talk.png"
image b_move:
    offset(-23, -8)
    "gui/button/selector_move.png"
image b_default:
    offset(-23, -8)
    "gui/button/selector_default.png"

screen choice(items):
    style_prefix "choice"

    vbox:
        for i in items:
            $ badge = i.kwargs.get("badge", None)

            button:
                hbox:
                    xalign 0.5 spacing -20
                    if badge:
                        add badge yalign 0.5 yoffset 10 at selector_apprear
                    else:
                        add "b_default" yalign 0.5 yoffset 10 at selector_apprear
                    text i.caption at choice_select
                action i.action
            #textbutton i.caption action i.action at choice_select


style choice_vbox:
    xalign 0.5
    ypos 405
    yanchor 0.5
    spacing 33

style choice_button:
    is default # This means it doesn't use the usual button styling
    xysize (1000, None)
    background Frame("gui/button/choice_idle.png",
        150, 50, 150, 50, tile=False)
    padding (40, 10)

style choice_text:
    is default # This means it doesn't use the usual button text styling
    xalign 0.5 yalign 0.5
    line_spacing -10 textalign 0.5
    idle_color light_accent
    hover_color white
    insensitive_color "#444"
