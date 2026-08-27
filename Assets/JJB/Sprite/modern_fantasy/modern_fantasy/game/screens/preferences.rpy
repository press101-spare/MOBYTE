
## Preferences screen ##########################################################
##
## The preferences screen allows the player to configure the game to better suit
## themselves.
##
## https://www.renpy.org/doc/html/screen_special.html#preferences

screen preferences():

    tag menu

    use game_menu(_("Preferences"))

    viewport:
        
        mousewheel True pagekeys True
        scrollbars "vertical"

        xsize config.screen_width-600
        ysize config.screen_height-350
        align (0.5, 0.5)
        offset(150, 0)

        vbox:
            spacing 15
            ###A lot of code for one button but don't get scared it's just a lot of containers


            ####Display button
            button:
                style_prefix "pref"
                hbox:
                    style_prefix "select"
                    label "Display"
                    button:
                        hbox:
                            spacing -10
                            add "gui/options/select_arrow.png" align(0.5, 0.5) at choice_select_mirror
                            if preferences.fullscreen == True:
                                text "Fullscreen" align(0.5, 0.5)
                            else:
                                text "Windowed" align(0.5, 0.5)
                            add "gui/options/select_arrow.png" xzoom -1 align(0.5, 0.5) at choice_select
                        action Preference("display", "toggle") 



            ###Music slider
            if config.has_music:
                button:
                    style_prefix "pref"
                    background "gui/options/btn_slider_base.png"
                    hbox:
                        style_prefix "select"
                        label "Music"
                        bar value Preference("music volume"):
                            xalign 1.0 xsize 811 xoffset -35


            ###Sound slider
            if config.has_sound:
                button:
                    style_prefix "pref"
                    background "gui/options/btn_slider_base.png"
                    hbox:
                        style_prefix "select"
                        label "Sound"
                        bar value Preference("sound volume"):
                            xalign 1.0 xsize 811 xoffset -35



            ###Voice slider
            if config.has_voice:
                button:
                    style_prefix "pref"
                    background "gui/options/btn_slider_base.png"
                    hbox:
                        style_prefix "select"
                        label "Voice"
                        bar value Preference("voice volume"):
                            xalign 1.0 xsize 811 xoffset -35



            ####Skip stuff button
            button:
                style_prefix "pref"
                hbox:
                    style_prefix "select"
                    label "Skip Unseen"
                    button:
                        hbox:
                            spacing -10
                            add "gui/options/select_arrow.png" align(0.5, 0.5) at choice_select_mirror
                            if preferences.skip_unseen == True:
                                text "On" align(0.5, 0.5)
                            else:
                                text "Off" align(0.5, 0.5)
                            add "gui/options/select_arrow.png" xzoom -1 align(0.5, 0.5) at choice_select
                        action Preference("skip", "toggle")



            button:
                style_prefix "pref"
                hbox:
                    style_prefix "select"
                    label "After Choices"
                    button:
                        hbox:
                            spacing -10
                            add "gui/options/select_arrow.png" align(0.5, 0.5) at choice_select_mirror
                            if preferences.skip_after_choices == True:
                                text "Keep Skipping" align(0.5, 0.5)
                            else:
                                text "Pause" align(0.5, 0.5)
                            add "gui/options/select_arrow.png" xzoom -1 align(0.5, 0.5) at choice_select
                        action Preference("after choices", "toggle")


            
            button:
                style_prefix "pref"
                hbox:
                    style_prefix "select"
                    label "Transitions"
                    button:
                        hbox:
                            spacing -10
                            add "gui/options/select_arrow.png" align(0.5, 0.5) at choice_select_mirror
                            if preferences.transitions == 2:
                                text "On" align(0.5, 0.5)
                            else:
                                text "Off" align(0.5, 0.5)
                            add "gui/options/select_arrow.png" xzoom -1 align(0.5, 0.5) at choice_select
                        action InvertSelected(Preference("transitions", "toggle"))
                        

        




style pref_button:
    xysize(1207,108) background "gui/options/btn_base.png"
style select_label:
    yalign 0.5 xoffset 60
style select_label_text:
    size 40
    color light_accent
    ###Outlines to create shadow effect
    outlines [ (1, "#2b262609", 0, 0),  (2, "#00000007", 0, 0), (3, "#00000010", 0, 0), (4, "#00000007", 0, 0) ]

style select_button:
    xalign 1.0
    xysize(634,108) background "gui/options/select_base.png"
style select_hbox:
    xfill True
    align(0.5, 0.5)
style select_text:
    idle_color dark_accent
    hover_color white
    size 40

