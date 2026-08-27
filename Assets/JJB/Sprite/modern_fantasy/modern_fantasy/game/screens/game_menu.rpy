## Game Menu screen ############################################################
##
## This lays out the basic common structure of a game menu screen. It's called
## with the screen title, and displays the title and navigation.
##
## This screen no longer includes a background, and it no longer transcludes
## its contents. It is intended to be easily removable from any given menu
## screen and thus you are required to do some of the heavy lifting for
## setting up containers for the contents of your menu screens.
image mist1:
    "gui/game_menu/mist_1.png"
    zoom 4.0 alpha 0.7
    xpan 0
    linear 100.0 xpan 360.0
    repeat

image mist2:
    "gui/game_menu/mist_2.png"
    zoom 5.0 alpha 0.5
    xpan 0
    linear 150.0 xpan 360.0
    repeat


screen game_menu(title):

    add "mist1"
    add "mist2"
    add "gui/game_menu/bg.png"
    

    style_prefix "game_menu"

    vbox:
        xpos 80 yalign 0.5
        spacing 10

        if main_menu:

            textbutton _("Start") action Start() at choice_select

        else:

            textbutton _("History") action ShowMenu("history") at choice_select

        if main_menu:
            textbutton _("Savefiles") action ShowMenu("load") at choice_select
        else:
            textbutton _("Savefiles") action ShowMenu("save") at choice_select

        textbutton _("Preferences") action ShowMenu("preferences") at choice_select

        if _in_replay:

            textbutton _("End Replay") action EndReplay(confirm=True) at choice_select

        elif not main_menu:

            textbutton _("Main Menu") action MainMenu() at choice_select

        textbutton _("About") action ShowMenu("about") at choice_select

        if renpy.variant("pc") or (renpy.variant("web") and not renpy.variant("mobile")):

            ## Help isn't necessary or relevant to mobile devices.
            textbutton _("Help") action ShowMenu("help") at choice_select

        if renpy.variant("pc"):

            ## The quit button is banned on iOS and
            ## unnecessary on Android and Web.
            textbutton _("Quit") action Quit(confirm=not main_menu) at choice_select



    button:
        background None
        xalign 1.0 offset(-130, 30)
        vbox:
            spacing 10
            add "gui/return_bord.png"
            hbox:
                xalign 0.5 spacing 20
                button:
                    xysize(99,45)
                    add "gui/ret_bg.png"
                    text "Esc" size 42 color light_accent align(0.5, 0.5) yoffset 5
                text "Back" size 42 idle_color light_accent hover_color white yalign 0.5 yoffset 3
            add "gui/return_bord.png"

        action Return()
        at smaller_button

transform smaller_button:
    zoom 0.8
style return_button:
    xpos 60
    yalign 1.0
    yoffset -45

image nav_selected:
    "gui/game_menu/nav_btn.png"
    alpha 0.2

style game_menu_button:
    hover_background "gui/game_menu/nav_btn.png"
    selected_background "nav_selected"
    xysize(248,83)

style game_menu_button_text:
    size 35
    xoffset 40
    yalign 0.5
    idle_color light_accent
    hover_color white
    selected_color white
    ###Outlines to create shadow effect
    outlines [ (1, "#2b262609", 0, 0),  (2, "#00000007", 0, 0), (3, "#00000010", 0, 0), (4, "#00000007", 0, 0) ]

style game_menu_viewport:
    xsize config.screen_width-420
    ysize config.screen_height-350
    align (0.5, 0.5)

style game_menu_side:
    yfill True
    align (1.0, 0.5)

style game_menu_vscrollbar:
    unscrollable "hide"

style game_menu_label:
    padding (10, 10)
style game_menu_label_text:
    size 45
