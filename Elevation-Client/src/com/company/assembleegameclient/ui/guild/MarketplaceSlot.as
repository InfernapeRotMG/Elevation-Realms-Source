/**
 * Created by club5_000 on 9/20/2014.
 */
package com.company.assembleegameclient.ui.guild {
import com.company.assembleegameclient.constants.InventoryOwnerTypes;
import com.company.assembleegameclient.game.AGameSprite;
import com.company.assembleegameclient.objects.ObjectLibrary;
import com.company.assembleegameclient.ui.tooltip.EquipmentToolTip;
import com.company.util.GraphicsUtil;
import flash.display.Bitmap;
import flash.display.BitmapData;
import flash.display.GraphicsPath;
import flash.display.GraphicsSolidFill;
import flash.display.IGraphicsData;
import flash.display.Sprite;
import flash.events.Event;
import flash.events.MouseEvent;
import flash.geom.Point;
import kabam.rotmg.core.signals.HideTooltipsSignal;
import kabam.rotmg.core.signals.ShowTooltipSignal;
import kabam.rotmg.tooltips.HoverTooltipDelegate;
import kabam.rotmg.tooltips.TooltipAble;

public class MarketplaceSlot extends Sprite implements TooltipAble
{

    public static const WIDTH:int = 48;
    public static const HEIGHT:int = 48;

    public var gs_:AGameSprite;
    public var item_:int = -1;;
    public var data_:Object;
    public var itemBitmap_:Bitmap;
    public var toolTip_:EquipmentToolTip;
    public var enabled_:Boolean;
    public var slot_:int = 0;
    public var selectItem_:Boolean;
    private var disabledOverlay_:Sprite;
    protected var fill_:GraphicsSolidFill;
    protected var path_:GraphicsPath;
    private var graphicsData_:Vector.<IGraphicsData>;
    public var hoverTooltipDelegate:HoverTooltipDelegate;

    public function MarketplaceSlot(_arg1:int, _arg2:Object, _arg3:AGameSprite, _arg4:Boolean, _arg5:Boolean=false)
    {
        this.hoverTooltipDelegate = new HoverTooltipDelegate();
        this.fill_ = new GraphicsSolidFill(0x565656);
        this.path_ = new GraphicsPath(new Vector.<int>(), new Vector.<Number>());
        this.graphicsData_ = new <IGraphicsData>[this.fill_, this.path_, GraphicsUtil.END_FILL];
        super();
        this.gs_ = _arg3;
        this.data_ = _arg2;
        this.item_ = _arg1;
        this.enabled_ = _arg4;
        this.selectItem_ = _arg5;
        var _local2:XML;
        var _local3:Number;
        var _local4:BitmapData;
        var _local5:Point;
        this.graphicsData_ = new <IGraphicsData>[this.fill_, this.path_, GraphicsUtil.END_FILL];
        GraphicsUtil.clearPath(this.path_);
        GraphicsUtil.drawCutEdgeRect(0, 0, WIDTH, HEIGHT, 6, [1, 1, 1, 1], this.path_);
        graphics.clear();
        graphics.drawGraphicsData(this.graphicsData_);
        this.graphicsData_ = new <IGraphicsData>[this.fill_, this.path_, GraphicsUtil.END_FILL];
        graphics.drawGraphicsData(this.graphicsData_);
        if (((!((this.itemBitmap_ == null))) && (this.contains(this.itemBitmap_))))
        {
            removeChild(this.itemBitmap_);
            this.itemBitmap_ = null;
        };
        if (this.item_ != -1)
        {
            _local2 = ObjectLibrary.xmlLibrary_[this.item_];
            _local3 = 5;
            if (_local2.hasOwnProperty("ScaleValue"))
            {
                _local3 = _local2.ScaleValue;
            };
            _local4 = ObjectLibrary.getRedrawnTextureFromType(this.item_, 80, true);
            if (((((!((this.data_ == null))) && (this.data_.hasOwnProperty("TextureFile")))) && (!((this.data_.TextureFile == "")))))
            {
                _local4 = ObjectLibrary.getRedrawnTextureFromTypeCustom(this.item_, 80, true, this.item_, true, _local3, 0, 0);
            };
            this.itemBitmap_ = new Bitmap(_local4);
            _local5 = this._H_K_(this.item_, ObjectLibrary.getSlotTypeFromType(this.item_), false);
            this.itemBitmap_.x = (((-(this.itemBitmap_.width) / 2) + (WIDTH / 2)) + _local5.x);
            this.itemBitmap_.y = (((-(this.itemBitmap_.height) / 2) + (HEIGHT / 2)) + _local5.y);
            addChild(this.itemBitmap_);
            this.hoverTooltipDelegate.setDisplayObject(this);
            this.toolTip_ = new EquipmentToolTip(this.item_, gs_.map.player_, -1, InventoryOwnerTypes.NPC);
            this.toolTip_.forcePostionRight();
            this.hoverTooltipDelegate.tooltip = this.toolTip_;
        };
        if (((!((this.disabledOverlay_ == null))) && (this.contains(this.disabledOverlay_))))
        {
            removeChild(this.disabledOverlay_);
            this.disabledOverlay_ = null;
        };
        if (!(this.enabled_))
        {
            this.disabledOverlay_ = new Sprite();
            this.disabledOverlay_.graphics.beginFill(0, 0.5);
            this.disabledOverlay_.graphics.moveTo(6, 0);
            this.disabledOverlay_.graphics.lineTo((WIDTH - 6), 0);
            this.disabledOverlay_.graphics.lineTo(WIDTH, 6);
            this.disabledOverlay_.graphics.lineTo(WIDTH, (HEIGHT - 6));
            this.disabledOverlay_.graphics.lineTo((WIDTH - 6), HEIGHT);
            this.disabledOverlay_.graphics.lineTo(6, HEIGHT);
            this.disabledOverlay_.graphics.lineTo(0, (HEIGHT - 6));
            this.disabledOverlay_.graphics.lineTo(0, 6);
            this.disabledOverlay_.graphics.lineTo(6, 0);
            this.disabledOverlay_.graphics.endFill();
            addChild(this.disabledOverlay_);
        };
    }


    public function redraw()
    {
        var _local2:XML;
        var _local3:Number;
        var _local4:BitmapData;
        var _local5:Point;
        this.graphicsData_ = new <IGraphicsData>[this.fill_, this.path_, GraphicsUtil.END_FILL];
        GraphicsUtil.clearPath(this.path_);
        GraphicsUtil.drawCutEdgeRect(0, 0, WIDTH, HEIGHT, 6, [1, 1, 1, 1], this.path_);
        graphics.clear();
        graphics.drawGraphicsData(this.graphicsData_);
        this.graphicsData_ = new <IGraphicsData>[this.fill_, this.path_, GraphicsUtil.END_FILL];
        graphics.drawGraphicsData(this.graphicsData_);
        if (((!((this.itemBitmap_ == null))) && (this.contains(this.itemBitmap_))))
        {
            removeChild(this.itemBitmap_);
            this.itemBitmap_ = null;
        };
        if (this.item_ != -1)
        {
            _local2 = ObjectLibrary.xmlLibrary_[this.item_];
            _local3 = 5;
            if (_local2.hasOwnProperty("ScaleValue"))
            {
                _local3 = _local2.ScaleValue;
            };
            _local4 = ObjectLibrary.getRedrawnTextureFromType(this.item_, 80, true);
            if (((((!((this.data_ == null))) && (this.data_.hasOwnProperty("TextureFile")))) && (!((this.data_.TextureFile == "")))))
            {
                _local4 = ObjectLibrary.getRedrawnTextureFromTypeCustom(this.item_, 80, true, this.item_, true, _local3, 0, 0);
            };
            this.itemBitmap_ = new Bitmap(_local4);
            _local5 = this._H_K_(this.item_, ObjectLibrary.getSlotTypeFromType(this.item_), false);
            this.itemBitmap_.x = (((-(this.itemBitmap_.width) / 2) + (WIDTH / 2)) + _local5.x);
            this.itemBitmap_.y = (((-(this.itemBitmap_.height) / 2) + (HEIGHT / 2)) + _local5.y);
            this.toolTip_.forcePostionLeft();
            addChild(this.itemBitmap_);
            this.hoverTooltipDelegate.setDisplayObject(this);
            this.toolTip_ = new EquipmentToolTip(this.item_, gs_.map.player_, -1, InventoryOwnerTypes.NPC);
            this.toolTip_.forcePostionRight();
            this.hoverTooltipDelegate.tooltip = this.toolTip_;
        };
        if (((!((this.disabledOverlay_ == null))) && (this.contains(this.disabledOverlay_))))
        {
            removeChild(this.disabledOverlay_);
            this.disabledOverlay_ = null;
        };
        if (!(this.enabled_))
        {
            this.disabledOverlay_ = new Sprite();
            this.disabledOverlay_.graphics.beginFill(0, 0.5);
            this.disabledOverlay_.graphics.moveTo(6, 0);
            this.disabledOverlay_.graphics.lineTo((WIDTH - 6), 0);
            this.disabledOverlay_.graphics.lineTo(WIDTH, 6);
            this.disabledOverlay_.graphics.lineTo(WIDTH, (HEIGHT - 6));
            this.disabledOverlay_.graphics.lineTo((WIDTH - 6), HEIGHT);
            this.disabledOverlay_.graphics.lineTo(6, HEIGHT);
            this.disabledOverlay_.graphics.lineTo(0, (HEIGHT - 6));
            this.disabledOverlay_.graphics.lineTo(0, 6);
            this.disabledOverlay_.graphics.lineTo(6, 0);
            this.disabledOverlay_.graphics.endFill();
            addChild(this.disabledOverlay_);
        };
    }

    protected function _H_K_(_arg1:int, _arg2:int, _arg3:Boolean):Point
    {
        var _local4:Point = new Point();
        switch (_arg2)
        {
            case 9:
                _local4.x = (((_arg1)==2878) ? 0 : -2);
                _local4.y = ((_arg3) ? -2 : 0);
                break;
            case 11:
                _local4.y = -2;
                break;
        };
        return (_local4);
    }


    public function setShowToolTipSignal(_arg_1:ShowTooltipSignal):void {
        this.hoverTooltipDelegate.setShowToolTipSignal(_arg_1);
    }

    public function getShowToolTip():ShowTooltipSignal {
        return (this.hoverTooltipDelegate.getShowToolTip());
    }

    public function setHideToolTipsSignal(_arg_1:HideTooltipsSignal):void {
        this.hoverTooltipDelegate.setHideToolTipsSignal(_arg_1);
    }

    public function getHideToolTips():HideTooltipsSignal {
        return (this.hoverTooltipDelegate.getHideToolTips());
    }


}
}