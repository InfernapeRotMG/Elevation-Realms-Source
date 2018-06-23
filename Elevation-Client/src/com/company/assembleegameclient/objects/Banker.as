package com.company.assembleegameclient.objects {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.ui.panels.Panel;

import com.company.assembleegameclient.ui.panels.BankerPanel;

public class Banker extends GameObject implements IInteractiveObject {

    public function Banker(_arg_1:XML) {
        super(_arg_1);
        isInteractive_ = true;
    }

    public function getPanel(_arg_1:GameSprite):Panel {
        return (new BankerPanel(_arg_1));
    }
}
}//package com.company.assembleegameclient.objects
