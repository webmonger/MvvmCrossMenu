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
            // [[UIDynamicAnimator alloc] initWithCollectionViewLayout:self];
            _visibleIndexPathsSet = new List<NSIndexPath>(); // [NSMutableSet set];
            _visibleHeaderAndFooterSet = new List<NSIndexPath>(); // [[NSMutableSet alloc] init];
        }

        public override void PrepareLayout()
        {
            base.PrepareLayout();

            ScrollDirection = UICollectionViewScrollDirection.Vertical;


            if (UIApplication.SharedApplication.StatusBarOrientation != _interfaceOrientation)
                //if ([[UIApplication sharedApplication] statusBarOrientation] != self.interfaceOrientation) {
            {

                DynamicAnimator.RemoveAllBehaviors();
                //[self.dynamicAnimator removeAllBehaviors];
                _visibleIndexPathsSet = new List<NSIndexPath>();
//                visibleIndexPathsSet = [NSMutableSet set ];
            }

            _interfaceOrientation = UIApplication.SharedApplication.StatusBarOrientation;
            //self.interfaceOrientation = [[UIApplication sharedApplication] statusBarOrientation];

            // Need to overflow our actual visible rect slightly to avoid flickering.
            RectangleF visibleRect = new RectangleF(CollectionView.Frame.X, CollectionView.Frame.Y,
                CollectionView.Frame.Width + 100, CollectionView.Frame.Height + 100);
            //CGRect visibleRect = CGRectInset((CGRect){.origin = self.collectionView.bounds.origin, .size = self.collectionView.frame.size}, -100, -100);

            List<UICollectionViewLayoutAttributes> itemsInVisibleRectArray =
                base.LayoutAttributesForElementsInRect(visibleRect).ToList();
            //NSArray *itemsInVisibleRectArray = [super layoutAttributesForElementsInRect:visibleRect];

            List<NSIndexPath> itemsIndexPathsInVisibleRectSet =
                itemsInVisibleRectArray.Select(
                    x => x.IndexPath).ToList();
            //NSSet *itemsIndexPathsInVisibleRectSet = [NSSet setWithArray:[itemsInVisibleRectArray valueForKey:@"indexPath"]];

//            List<UICollectionViewLayoutAttributes> noLongerVisibleAttributes =
//                new List<UICollectionViewLayoutAttributes>();
            var noLongerVisibleBehaviours = new List<UIAttachmentBehavior>();


            // Step 1: Remove any behaviours that are no longer visible.
            for (int i = 0; i < DynamicAnimator.Behaviors.Count(); i++)
            {
                var item = DynamicAnimator.Behaviors[i] as UIAttachmentBehavior;
                var indexPath = CollectionView.IndexPathForCell(item.Items.FirstOrDefault() as UICollectionViewCell);
                if (!itemsIndexPathsInVisibleRectSet.Contains(indexPath))
                {
                    noLongerVisibleBehaviours.Add(item);
                }
            }
//            var noLongerVisibleBehaviours =
//                DynamicAnimator.Behaviors.Where(x => !itemsIndexPathsInVisibleRectSet.Contains(x));
            //NSArray *noLongerVisibleBehaviours = [self.dynamicAnimator.behaviors filteredArrayUsingPredicate:[NSPredicate predicateWithBlock:^BOOL(UIAttachmentBehavior *behaviour, NSDictionary *bindings) {
            //    return [itemsIndexPathsInVisibleRectSet containsObject:[[[behaviour items] firstObject] indexPath]] == NO;
            //}]];

            foreach (var behaviour in noLongerVisibleBehaviours)
            {
                DynamicAnimator.RemoveBehavior(behaviour);
                UICollectionViewLayoutAttributes attributes =
                    behaviour.Items.FirstOrDefault() as UICollectionViewLayoutAttributes;
                _visibleIndexPathsSet.Remove(attributes.IndexPath);
                _visibleHeaderAndFooterSet.Remove(attributes.IndexPath);
            }

//    [noLongerVisibleBehaviours enumerateObjectsUsingBlock:^(id obj, NSUInteger index, BOOL *stop) {
//        [self.dynamicAnimator removeBehavior:obj];

//        [self.visibleIndexPathsSet removeObject:[[[obj items]firstObject]indexPath]];


//        [self.visibleHeaderAndFooterSet removeObject:[[[obj items] firstObject] indexPath]];
//    }];

            // Step 2: Add any newly visible behaviours.
            // A "newly visible" item is one that is in the itemsInVisibleRect(Set|Array) but not in the visibleIndexPathsSet
            List<NSIndexPath> newlyVisibleItems = new List<NSIndexPath>();
            for (int i = 0; i < itemsInVisibleRectArray.Count(); i++)
            {
                var item = itemsInVisibleRectArray[i];
                newlyVisibleItems.Add((item.RepresentedElementCategory == UICollectionElementCategory.Cell)
                    ? _visibleIndexPathsSet[item.IndexPath.Row]
                    : _visibleHeaderAndFooterSet[item.IndexPath.Row]);
            }


//            List<NSIndexPath> newlyVisibleItems =
//                itemsInVisibleRectArray.Select(x => (x.RepresentedElementCategory == UICollectionElementCategory.Cell)
//                    ? _visibleIndexPathsSet.Skip(x.IndexPath.Row).FirstOrDefault()
//                    : _visibleHeaderAndFooterSet.Skip(x.IndexPath.Row).FirstOrDefault()).ToList();



//    NSArray *newlyVisibleItems = [itemsInVisibleRectArray filteredArrayUsingPredicate:[NSPredicate predicateWithBlock:^BOOL(UICollectionViewLayoutAttributes *item, NSDictionary *bindings) {
//        return (item.representedElementCategory == UICollectionElementCategoryCell ?
//                [self.visibleIndexPathsSet containsObject:item.indexPath] : [self.visibleHeaderAndFooterSet containsObject:item.indexPath]) == NO;
//    }]];

            PointF touchLocation = CollectionView.PanGestureRecognizer.LocationInView(CollectionView);


            for (int i = 0; i < newlyVisibleItems.Count(); i++)
            {

                var item = LayoutAttributesForItem(newlyVisibleItems.Skip(i).FirstOrDefault());
//            [newlyVisibleItems enumerateObjectsUsingBlock:^(UICollectionViewLayoutAttributes *item, NSUInteger idx, BOOL *stop) {
                PointF center = item.Center;
                UIAttachmentBehavior springBehaviour = new UIAttachmentBehavior(item, center);
//			UIAttachmentBehavior springBehaviour = [[UIAttachmentBehavior alloc] initWithItem:item attachedToAnchor:center];

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
//        [self.dynamicAnimator addBehavior:springBehaviour];
                    if (item.RepresentedElementCategory == UICollectionElementCategory.Cell)
                    {
                        _visibleIndexPathsSet.Add(item.IndexPath);
//            [self.visibleIndexPathsSet addObject:item.indexPath];
                    }
                    else
                    {
                        _visibleHeaderAndFooterSet.Add(item.IndexPath);
//            [self.visibleHeaderAndFooterSet addObject:item.indexPath];
                    }
//    }];
                }
            }
        }


        public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect(RectangleF rect)
        {
            var items = DynamicAnimator.GetDynamicItems(rect);

            return items.Select(x => x as UICollectionViewLayoutAttributes).ToArray();
        }

//        - (NSArray *)layoutAttributesForElementsInRect:(CGRect)rect {
//           return [self.dynamicAnimator itemsInRect:rect];
//        }

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
                //var item = DynamicAnimator.Behaviors [i];

                UIAttachmentBehavior springBehaviour = DynamicAnimator.Behaviors[i] as UIAttachmentBehavior;

                //[_dynamicAnimator.behaviors enumerateObjectsUsingBlock:^(UIAttachmentBehavior *springBehaviour, NSUInteger idx, BOOL *stop) {
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
                    UICollectionViewLayoutAttributes item =
                        springBehaviour.Items.FirstOrDefault() as UICollectionViewLayoutAttributes;
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
                    UICollectionViewLayoutAttributes item =
                        springBehaviour.Items.FirstOrDefault() as UICollectionViewLayoutAttributes;
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

//                UIAttachmentBehavior* springBehaviour = [[
//                UIAttachmentBehavior alloc ]
//                initWithItem:
//                attributes
//                attachedToAnchor:
//                attributes.center]
//                ;
//
//                [
//                self.dynamicAnimator
//                addBehavior:
//                springBehaviour]
//                ;
            }

        }
    }
}