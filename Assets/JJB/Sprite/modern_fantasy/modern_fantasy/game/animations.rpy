transform choice_select():
    on idle:
        easein 0.3 xoffset 0
    on hover:
        easein 0.3 xoffset 10
    on selected_hover:
        xoffset 10

transform choice_select_mirror():
    on idle:
        easein 0.3 xoffset 0
    on hover:
        easein 0.3 xoffset -10
    on selected_hover:
        xoffset -10

transform selector_apprear():
    alpha 0.0
    on idle:
        linear 0.2 alpha 0.0
    on hover:
        pause 0.15
        linear 0.3 alpha 1

define quick_dissolve = Dissolve(0.3)
