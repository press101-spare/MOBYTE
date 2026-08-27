
## Main Menu screen ############################################################
##
## Used to display the main menu when Ren'Py starts.
##
## https://www.renpy.org/doc/html/screen_special.html#main-menu


image main_menu_image = Solid("#8888") ###add your own main menu image here

screen main_menu():

    ## This ensures that any other menu screen is replaced.
    tag menu

    add "main_menu_image"


    add "mist1" alpha 0.1
    add "gui/main_menu.png"


    style_prefix "main"

    vbox:
        xpos 80
        yalign 0.5
        spacing 6

        textbutton _("New Game") action Start() text_font "fonts/SpaceAndAstronomy-pZRD.ttf" text_size 62 xoffset -35 at choice_select

        textbutton _("Continue") action ShowMenu("load") at choice_select

        textbutton _("Preferences") action ShowMenu("preferences") at choice_select

        textbutton _("About") action ShowMenu("about") at choice_select

        if renpy.variant("pc"):

            ## The quit button is banned on iOS and unnecessary on Android and
            ## Web.
            textbutton _("Quit") action Quit(confirm=not main_menu) at choice_select



style main_button_text:
    size 48
    outlines [ (1, "#2b262609", 0, 0),  (2, "#00000007", 0, 0), (3, "#00000010", 0, 0), (4, "#00000007", 0, 0) ]

