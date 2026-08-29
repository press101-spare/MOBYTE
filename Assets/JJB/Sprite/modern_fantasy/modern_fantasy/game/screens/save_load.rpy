## Load and Save screens #######################################################
##
## These screens are responsible for letting the player save the game and load
## it again. Since they share nearly everything in common, both are implemented
## in terms of a third screen, file_slots.
##
## https://www.renpy.org/doc/html/screen_special.html#save
## https://www.renpy.org/doc/html/screen_special.html#load


## The width and height of thumbnails used by the save slots.
define config.thumbnail_width = 698
define config.thumbnail_height = 392

define savefile_number = 20 ###how many savefiles do you want max?


###Function to display runtime
###https://www.renpy.org/doc/html/save_load_rollback.html#renpy.slot_json
init python:
    def show_game_runtime(slot):
        metadata = renpy.slot_json(slot)
        name = metadata.get('_version', '')
        runtime = metadata.get('_game_runtime', 0)
        hours = int(runtime // 3600)
        minutes = int((runtime % 3600) // 60)
        seconds = int(runtime % 60)
        runtime_formatted = f"{hours:02d}:{minutes:02d}:{seconds:02d}"
        #renpy.notify(slot)
        return runtime_formatted


    ###NECCESSARY VARIABLE to determine which fileslot is selected
    selected_slot = None


screen save():

    tag menu


    use file_slots


screen load():

    tag menu

    use file_slots

    

screen file_slots():

    use game_menu(_("Save"))

    vbox:
        style_prefix "save"
        label "Save Files"

        viewport:
            scrollbars "vertical" mousewheel True
            vbox:
                for i in range(savefile_number):
                    $ slot = i + 1

                    button:
                        style_prefix "slot_btn"
                        hbox:
                            
                            add "gui/saveload/save_bullet.png" yalign 0.5
                            if renpy.can_load('1-{}'.format(slot)):
                                text FileSlotName(slot, 20, auto='a', quick='q', format='%sSave %d —') yalign 0.5
                            text FileSaveName(slot, empty=_("Empty Slot")) yalign 0.5 xoffset -8

                        if renpy.can_load('1-{}'.format(slot)):
                            if not main_menu:
                                action [SetVariable("selected_slot", slot),Show("save", quick_dissolve), SelectedIf(selected_slot == slot)] ###the show stuff here is basically here so there's a nice dissolve transition when the player saves
        
                            else:
                                action [SetVariable("selected_slot", slot),Show("load", quick_dissolve), SelectedIf(selected_slot == slot)]
                        else:
                            action [FileSave(slot), SetVariable("selected_slot", slot), Show("save", quick_dissolve), SelectedIf(selected_slot == slot)]


    if selected_slot:

        vbox:
            align(1.0, 0.5) xoffset -80 spacing 20
            button:
                style_prefix "selslot"
                xysize(760,454)
                add "gui/saveload/thumbnail_border.png"
                add FileScreenshot(selected_slot) align(0.5, 0.5)

                textbutton "Load Game" action FileLoad(selected_slot):
                    text_align(0.5, 0.5) align(0.5, 1.0) yoffset 19
                    background Frame("gui/namebox.png", 60, 0, 60, 0, tile=False) xysize(324,98)
                    text_idle_color light_accent
                    text_hover_color white

            
            vbox:
                style_prefix "seltime"
                
                hbox:
                    label "Save Time"
                    label "Play Time" xalign 1.0

                hbox:
                    text FileTime(selected_slot, format=_("{#file_time}%Y.%m.%d. — %H:%M"), empty=_("empty slot"))

                    text "[show_game_runtime('1-{}'.format(selected_slot))]" xalign 1.0

                hbox:
                    textbutton "Overwrite Save" action FileSave(selected_slot)
                    textbutton "Delete Save" action [FileDelete(selected_slot), SetVariable("selected_slot", None)] xalign 1.0




style save_vbox:
    xpos(473) yalign 0.5 xysize(530, 874)
    spacing 17
style save_label:
    xalign 0.5
style save_label_text:
    color light_accent
    size 42
    outlines [ (1, "#2b262609", 0, 0),  (2, "#00000007", 0, 0), (3, "#00000010", 0, 0), (4, "#00000007", 0, 0) ]
style slot_btn_button:
    xsize 400
style slot_btn_text:
    idle_color white
    hover_color u"#ffff"
    selected_color u"#ffff"
    insensitive_color u"#888"
    size 35
    outlines [ (1, "#2b262609", 0, 0),  (2, "#00000007", 0, 0), (3, "#00000010", 0, 0), (4, "#00000007", 0, 0) ]
style slot_btn_hbox:
    spacing 15
style seltime_label_text:
    color light_accent
    size 42
    outlines [ (1, "#2b262609", 0, 0),  (2, "#00000007", 0, 0), (3, "#00000010", 0, 0), (4, "#00000007", 0, 0) ]
style seltime_text:
    color white
    size 37
    outlines [ (1, "#2b262609", 0, 0),  (2, "#00000007", 0, 0), (3, "#00000010", 0, 0), (4, "#00000007", 0, 0) ]
style seltime_hbox:
    xalign 0.5 xoffset 27
    xsize 700

style seltime_button_text:
    idle_color light_accent
    hover_color white
    size 42
    outlines [ (1, "#2b262609", 0, 0),  (2, "#00000007", 0, 0), (3, "#00000010", 0, 0), (4, "#00000007", 0, 0) ]

    
    



