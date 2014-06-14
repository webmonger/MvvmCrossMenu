using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Cirrious.CrossCore.Core;
using MonoTouch.CoreAnimation;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MvvmCrossMenu.Touch.Helpers
{
    public class SpringFlowLayout : UICollectionViewFlowLayout
    {
        private List<NSIndexPath> _visibleIndexPathsSet;
        private List<NSIndexPath> _visibleHeaderAndFooterSet;
        private UIInterfaceOrientation _interfaceOrientation;
        private float _latestDelta;
		private const float KScrollResistanceFactorDefault = 900.0f;

        public float ScrollResistanceFactor { get; set; }

        public UIDynamicAnimator DynamicAnimator { get; set; }

        public SpringFlowLayout()
        {
			DynamicAnimator = new UIDynamicAnimator(this);
			//ItemSize = new SizeF(270.0f, 50.0f);
            _visibleIndexPathsSet = new List<NSIndexPath>();
            _visibleHeaderAndFooterSet = new List<NSIndexPath>();
        }

        public override void PrepareLayout()
        {
            base.PrepareLayout();

            ScrollDirection = UICollectionViewScrollDirection.Vertical;


            if (UIApplication.SharedApplication.StatusBarOrientation != _interfaceOrientation)
            {
                DynamicAnimator.RemoveAllBehaviors();
                _visibleIndexPathsSet = new List<NSIndexPath>();
            }

            _interfaceOrientation = UIApplication.SharedApplication.StatusBarOrientation;

			// Need to overflow our actual visible rect slightly to avoid flickering.
			RectangleF visibleRect = RectangleF.Inflate(CollectionView.Bounds, +100 , +100);

			List<UICollectionViewLayoutAttributes> itemsInVisibleRectArray =
                base.LayoutAttributesForElementsInRect(visibleRect).ToList();
            
			List<NSIndexPath> itemsIndexPathsInVisibleRectSet =
                itemsInVisibleRectArray.Select(
                    x => x.IndexPath).ToList();
           
			var noLongerVisibleBehaviours = new List<UIAttachmentBehavior>();

            // Step 1: Remove any behaviours that are no longer visible.
            for (int i = 0; i < DynamicAnimator.Behaviors.Count(); i++)
            {
				var item = DynamicAnimator.Behaviors[i] as UIAttachmentBehavior;
                var indexPath = CollectionView.IndexPathsForVisibleItems.FirstOrDefault();//.IndexPathForCell(item.Items.First() );
                if (!itemsIndexPathsInVisibleRectSet.Contains(indexPath))
                {
                    noLongerVisibleBehaviours.Add(item);
                }
            }

			foreach (var behaviour in noLongerVisibleBehaviours)
            {
                DynamicAnimator.RemoveBehavior(behaviour);
                UICollectionViewLayoutAttributes attributes =
                    behaviour.Items.FirstOrDefault() as UICollectionViewLayoutAttributes;
                _visibleIndexPathsSet.Remove(attributes.IndexPath);
                _visibleHeaderAndFooterSet.Remove(attributes.IndexPath);
            }

            // Step 2: Add any newly visible behaviours.
            // A "newly visible" item is one that is in the itemsInVisibleRect(Set|Array) but not in the visibleIndexPathsSet
            List<NSIndexPath> newlyVisibleItems = new List<NSIndexPath>();
            for (int i = 0; i < itemsInVisibleRectArray.Count(); i++)
            {
                var item = itemsInVisibleRectArray[i];
                NSIndexPath indexPath = null;
                if (item.RepresentedElementCategory == UICollectionElementCategory.Cell)
                {
                    if (!_visibleIndexPathsSet.Contains(item.IndexPath))
                        indexPath = item.IndexPath;
                }
                else
                {
                    if (!_visibleHeaderAndFooterSet.Contains(item.IndexPath))
                        indexPath = item.IndexPath;
                }
                if (indexPath != null)
                    newlyVisibleItems.Add(indexPath);
            }
				
            PointF touchLocation = CollectionView.PanGestureRecognizer.LocationInView(CollectionView);


            for (int i = 0; i < newlyVisibleItems.Count(); i++)
            {
                var item = LayoutAttributesForItem(newlyVisibleItems.Skip(i).FirstOrDefault());
				PointF center = item.Center;
                UIAttachmentBehavior springBehaviour = new UIAttachmentBehavior(item, center);

				springBehaviour.Length = 1.0f;
                springBehaviour.Damping = 0.8f;
                springBehaviour.Frequency = 1.0f;

                // If our touchLocation is not (0,0), we'll need to adjust our item's center "in flight"
                if (PointF.Empty != touchLocation)
                {
                    if (ScrollDirection == UICollectionViewScrollDirection.Vertical)
                    {
                        float distanceFromTouch = touchLocation.Y - springBehaviour.AnchorPoint.Y;

                        float scrollResistance;
                        if (Math.Abs(ScrollResistanceFactor) > 0.0)
                        {
                            scrollResistance = distanceFromTouch/ScrollResistanceFactor;
                        }
                        else
                        {
                            scrollResistance = distanceFromTouch/KScrollResistanceFactorDefault;
                        }

                        if (_latestDelta < 0)
                        {
                            center.Y += Math.Max(_latestDelta, _latestDelta*scrollResistance);
                        }
                        else
                        {
                            center.Y += Math.Min(_latestDelta, _latestDelta*scrollResistance);
                        }

                        item.Center = center;
                    }
                    else
                    {
                        float distanceFromTouch = touchLocation.X - springBehaviour.AnchorPoint.X;

                        float scrollResistance;
                        if (Math.Abs(ScrollResistanceFactor) > 0.0)
                        {
                            scrollResistance = distanceFromTouch/ScrollResistanceFactor;
                        }
                        else
                        {
                            scrollResistance = distanceFromTouch/KScrollResistanceFactorDefault;
                        }
                        if (_latestDelta < 0)
                        {
                            center.X += Math.Max(_latestDelta, _latestDelta*scrollResistance);
                        }
                        else
                        {
                            center.X += Math.Min(_latestDelta, _latestDelta*scrollResistance);
                        }
                        item.Center = center;
                    }



                    DynamicAnimator.AddBehavior(springBehaviour);

					if (item.RepresentedElementCategory == UICollectionElementCategory.Cell)
                    {
                        _visibleIndexPathsSet.Add(item.IndexPath);
					}
                    else
                    {
                        _visibleHeaderAndFooterSet.Add(item.IndexPath);
					}
                }
            }
        }


        public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect(RectangleF rect)
		{
			var items = DynamicAnimator.GetDynamicItems(rect);
			return items.Select(x => x as UICollectionViewLayoutAttributes).ToArray();
        }

        public override UICollectionViewLayoutAttributes LayoutAttributesForItem(NSIndexPath indexPath)
        {
            if (indexPath == null)
                return base.LayoutAttributesForItem(indexPath);

            UICollectionViewLayoutAttributes dynamicLayoutAttributes =
                DynamicAnimator.GetLayoutAttributesForCell(indexPath);

            // Check if dynamic animator has layout attributes for a layout, otherwise use the flow layouts properties. This will prevent crashing when you add items later in a performBatchUpdates block (e.g. triggered by NSFetchedResultsController update)
            return dynamicLayoutAttributes ?? base.LayoutAttributesForItem(indexPath);
        }

        public override bool ShouldInvalidateLayoutForBoundsChange(RectangleF newBounds)
        {
            UIScrollView scrollView = CollectionView;

            Single delta;
            if (ScrollDirection == UICollectionViewScrollDirection.Vertical)
            {
                delta = newBounds.Y - scrollView.Bounds.Y;
            }
            else
            {
                delta = newBounds.X - scrollView.Bounds.X;
            }
            _latestDelta = delta;

            PointF touchLocation = CollectionView.PanGestureRecognizer.LocationInView(CollectionView);

            for (int i = 0; i < DynamicAnimator.Behaviors.Length; i++)
            {
				UIAttachmentBehavior springBehaviour = DynamicAnimator.Behaviors[i] as UIAttachmentBehavior;

				UICollectionViewLayoutAttributes item =
					springBehaviour.Items.FirstOrDefault() as UICollectionViewLayoutAttributes;

                if (ScrollDirection == UICollectionViewScrollDirection.Vertical)
                {
                    Single distanceFromTouch = touchLocation.Y - springBehaviour.AnchorPoint.Y;

                    Single scrollResistance;
                    if (Math.Abs(ScrollResistanceFactor) > 0.0)
                    {
                        scrollResistance = distanceFromTouch/ScrollResistanceFactor;
                    }
                    else
                    {
                        scrollResistance = distanceFromTouch/KScrollResistanceFactorDefault;
                    }
                    PointF center = item.Center;
                    if (delta < 0)
                    {
                        center.Y += Math.Max(delta, delta*scrollResistance);
                    }
                    else
                    {
                        center.Y += Math.Min(delta, delta*scrollResistance);
                    }

                    item.Center = center;

                    DynamicAnimator.UpdateItemUsingCurrentState(item);
                }
                else
                {
                    Single distanceFromTouch = touchLocation.X - springBehaviour.AnchorPoint.X;

                    Single scrollResistance;
                    if (Math.Abs(ScrollResistanceFactor) > 0.0)
                    {
                        scrollResistance = distanceFromTouch/ScrollResistanceFactor;
                    }
                    else
                    {
                        scrollResistance = distanceFromTouch/KScrollResistanceFactorDefault;
                    }
                    PointF center = item.Center;
                    if (delta < 0)
                    {
                        center.X += Math.Max(delta, delta*scrollResistance);
                    }
                    else
                    {
                        center.X += Math.Min(delta, delta*scrollResistance);
                    }

                    item.Center = center;

                    DynamicAnimator.UpdateItemUsingCurrentState(item);
                }
            }

            return false;
        }

        public override void PrepareForCollectionViewUpdates(UICollectionViewUpdateItem[] updateItems)
        {
            base.PrepareForCollectionViewUpdates(updateItems);

            for (int i = 0; i < updateItems.Length; i++)
            {
                var updateItem = updateItems[i];

                if (updateItem.UpdateAction == UICollectionUpdateAction.Insert)
                    return;

                UICollectionViewLayoutAttributes attributes = LayoutAttributesForItem(updateItem.IndexPathAfterUpdate);

                var springBehaviour = new UIAttachmentBehavior(attributes, attributes.Center);

                springBehaviour.Length = 1.0f;
                springBehaviour.Damping = 0.8f;
                springBehaviour.Frequency = 1.0f;

                DynamicAnimator.AddBehavior(springBehaviour);
            }

        }
    }
}
