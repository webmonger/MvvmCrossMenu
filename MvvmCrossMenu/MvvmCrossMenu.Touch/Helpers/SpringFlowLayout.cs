using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using MonoTouch.CoreAnimation;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MvvmCrossMenu.Touch.Helpers
{
    public class SpringFlowLayout : UICollectionViewFlowLayout
    {
        private UIDynamicAnimator _dynamicAnimator;
        private NSMutableSet _visibleIndexPathsSet;
        private NSMutableSet _visibleHeaderAndFooterSet;
        private UIInterfaceOrientation _interfaceOrientation;


        public SpringFlowLayout()
        {
    _dynamicAnimator = new UIDynamicAnimator(this);// [[UIDynamicAnimator alloc] initWithCollectionViewLayout:self];
    _visibleIndexPathsSet = new NSMutableSet();// [NSMutableSet set];
    _visibleHeaderAndFooterSet = new NSMutableSet();// [[NSMutableSet alloc] init];
        }
        
        public override void PrepareLayout()
        {
            base.PrepareLayout();

            if (UIApplication.SharedApplication.StatusBarOrientation != _interfaceOrientation)
            //if ([[UIApplication sharedApplication] statusBarOrientation] != self.interfaceOrientation) {
            {
                
                _dynamicAnimator.RemoveAllBehaviors();
                //[self.dynamicAnimator removeAllBehaviors];
                _visibleIndexPathsSet = new NSMutableSet();
                //visibleIndexPathsSet = [NSMutableSet set ];
            }

            _interfaceOrientation = UIApplication.SharedApplication.StatusBarOrientation;
    //self.interfaceOrientation = [[UIApplication sharedApplication] statusBarOrientation];
    
    // Need to overflow our actual visible rect slightly to avoid flickering.
            RectangleF visibleRect = new RectangleF(CollectionView.Frame.X, CollectionView.Frame.Y, -100, -100);
    //CGRect visibleRect = CGRectInset((CGRect){.origin = self.collectionView.bounds.origin, .size = self.collectionView.frame.size}, -100, -100);
    
    var itemsInVisibleRectArray = base.LayoutAttributesForElementsInRect(visibleRect);
    //NSArray *itemsInVisibleRectArray = [super layoutAttributesForElementsInRect:visibleRect];
    
    NSSet itemsIndexPathsInVisibleRectSet = NSSet.MakeNSObjectSet(itemsInVisibleRectArray);
    //NSSet *itemsIndexPathsInVisibleRectSet = [NSSet setWithArray:[itemsInVisibleRectArray valueForKey:@"indexPath"]];
    
    // Step 1: Remove any behaviours that are no longer visible.
    var noLongerVisibleBehaviours = this._dynamicAnimator.Behaviors.Select(x => x.ChildBehaviors);
    //NSArray *noLongerVisibleBehaviours = [self.dynamicAnimator.behaviors filteredArrayUsingPredicate:[NSPredicate predicateWithBlock:^BOOL(UIAttachmentBehavior *behaviour, NSDictionary *bindings) {
    //    return [itemsIndexPathsInVisibleRectSet containsObject:[[[behaviour items] firstObject] indexPath]] == NO;
    //}]];
    
//    [noLongerVisibleBehaviours enumerateObjectsUsingBlock:^(id obj, NSUInteger index, BOOL *stop) {
//        [self.dynamicAnimator removeBehavior:obj];
//        [self.visibleIndexPathsSet removeObject:[[[obj items] firstObject] indexPath]];
//        [self.visibleHeaderAndFooterSet removeObject:[[[obj items] firstObject] indexPath]];
//    }];

    // Step 2: Add any newly visible behaviours.
    // A "newly visible" item is one that is in the itemsInVisibleRect(Set|Array) but not in the visibleIndexPathsSet
            var newlyVisibleItems =
                itemsInVisibleRectArray.Select(
                    x =>
                        (x.RepresentedElementCategory == UICollectionElementCategory.Cell)
                            ? _visibleIndexPathsSet[x.IndexPath.Row)]
                            : _visibleHeaderAndFooterSet[x.IndexPath]);
//    NSArray *newlyVisibleItems = [itemsInVisibleRectArray filteredArrayUsingPredicate:[NSPredicate predicateWithBlock:^BOOL(UICollectionViewLayoutAttributes *item, NSDictionary *bindings) {
//        return (item.representedElementCategory == UICollectionElementCategoryCell ?
//                [self.visibleIndexPathsSet containsObject:item.indexPath] : [self.visibleHeaderAndFooterSet containsObject:item.indexPath]) == NO;
//    }]];

            PointF touchLocation = CollectionView.PanGestureRecognizer.LocationInView(CollectionView);
    //CGPoint touchLocation = [self.collectionView.panGestureRecognizer locationInView:self.collectionView];


            //newlyVisibleItems.Select();
//            [newlyVisibleItems enumerateObjectsUsingBlock:^(UICollectionViewLayoutAttributes *item, NSUInteger idx, BOOL *stop) {
        CGPoint center = item.center;
        UIAttachmentBehavior *springBehaviour = [[UIAttachmentBehavior alloc] initWithItem:item attachedToAnchor:center];
        
        springBehaviour.length = 1.0f;
        springBehaviour.damping = 0.8f;
        springBehaviour.frequency = 1.0f;
        
        // If our touchLocation is not (0,0), we'll need to adjust our item's center "in flight"
        if (!CGPointEqualToPoint(CGPointZero, touchLocation)) {
            if (self.scrollDirection == UICollectionViewScrollDirectionVertical) {
                CGFloat distanceFromTouch = fabsf(touchLocation.y - springBehaviour.anchorPoint.y);
                
                CGFloat scrollResistance;
                if (self.scrollResistanceFactor) scrollResistance = distanceFromTouch / self.scrollResistanceFactor;
                else scrollResistance = distanceFromTouch / kScrollResistanceFactorDefault;
                
                if (self.latestDelta < 0) center.y += MAX(self.latestDelta, self.latestDelta*scrollResistance);
                else center.y += MIN(self.latestDelta, self.latestDelta*scrollResistance);
                
                item.center = center;
                
            } else {
                CGFloat distanceFromTouch = fabsf(touchLocation.x - springBehaviour.anchorPoint.x);
                
                CGFloat scrollResistance;
                if (self.scrollResistanceFactor) scrollResistance = distanceFromTouch / self.scrollResistanceFactor;
                else scrollResistance = distanceFromTouch / kScrollResistanceFactorDefault;
                
                if (self.latestDelta < 0) center.x += MAX(self.latestDelta, self.latestDelta*scrollResistance);
                else center.x += MIN(self.latestDelta, self.latestDelta*scrollResistance);
                
                item.center = center;
            }
        }
        
        [self.dynamicAnimator addBehavior:springBehaviour];
        if(item.representedElementCategory == UICollectionElementCategoryCell)
        {
            [self.visibleIndexPathsSet addObject:item.indexPath];
        }
        else
        {
            [self.visibleHeaderAndFooterSet addObject:item.indexPath];
        }
    }];


        }

        public override bool ShouldInvalidateLayoutForBoundsChange(RectangleF newBounds)
        {
            return true;
        }

        public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect(RectangleF rect)
        {
            var array = base.LayoutAttributesForElementsInRect(rect);
            var visibleRect = new RectangleF(CollectionView.ContentOffset, CollectionView.Bounds.Size);

            foreach (var attributes in array)
            {
                if (attributes.Frame.IntersectsWith(rect))
                {
                    float distance = visibleRect.GetMidX() - attributes.Center.X;
                    float normalizedDistance = distance/ACTIVE_DISTANCE;
                    if (Math.Abs(distance) < ACTIVE_DISTANCE)
                    {
                        float zoom = 1 + ZOOM_FACTOR*(1 - Math.Abs(normalizedDistance));
                        attributes.Transform3D = CATransform3D.MakeScale(zoom, zoom, 1.0f);
                        attributes.ZIndex = 1;
                    }
                }
            }

            return array;
        }

        public override PointF TargetContentOffset(PointF proposedContentOffset, PointF scrollingVelocity)
        {
            float offSetAdjustment = float.MaxValue;
            float horizontalCenter = (float) (proposedContentOffset.X + (this.CollectionView.Bounds.Size.Width/2.0));
            RectangleF targetRect = new RectangleF(proposedContentOffset.X, 0.0f, this.CollectionView.Bounds.Size.Width,
                this.CollectionView.Bounds.Size.Height);
            var array = base.LayoutAttributesForElementsInRect(targetRect);
            foreach (var layoutAttributes in array)
            {
                float itemHorizontalCenter = layoutAttributes.Center.X;
                if (Math.Abs(itemHorizontalCenter - horizontalCenter) < Math.Abs(offSetAdjustment))
                {
                    offSetAdjustment = itemHorizontalCenter - horizontalCenter;
                }
            }
            return new PointF(proposedContentOffset.X + offSetAdjustment, proposedContentOffset.Y);
        }
    }