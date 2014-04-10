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
		private int? scrollResistanceFactor = null;
		private Single _latestDelta;


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
				itemsInVisibleRectArray.Select (
					x =>
                        (x.RepresentedElementCategory == UICollectionElementCategory.Cell)
					? _visibleIndexPathsSet.Skip (x.IndexPath.Row).FirstOrDefault ()
					: _visibleHeaderAndFooterSet.Skip (x.IndexPath.Row).FirstOrDefault ());
//    NSArray *newlyVisibleItems = [itemsInVisibleRectArray filteredArrayUsingPredicate:[NSPredicate predicateWithBlock:^BOOL(UICollectionViewLayoutAttributes *item, NSDictionary *bindings) {
//        return (item.representedElementCategory == UICollectionElementCategoryCell ?
//                [self.visibleIndexPathsSet containsObject:item.indexPath] : [self.visibleHeaderAndFooterSet containsObject:item.indexPath]) == NO;
//    }]];

            PointF touchLocation = CollectionView.PanGestureRecognizer.LocationInView(CollectionView);
    //CGPoint touchLocation = [self.collectionView.panGestureRecognizer locationInView:self.collectionView];


			for(int i = 0; i< newlyVisibleItems.Count(); i++){

				UICollectionViewLayoutAttributes item = newlyVisibleItems [i];
//            [newlyVisibleItems enumerateObjectsUsingBlock:^(UICollectionViewLayoutAttributes *item, NSUInteger idx, BOOL *stop) {
				PointF center = item.Center;
				UIAttachmentBehavior springBehaviour = new UIAttachmentBehavior (item, center);
//			UIAttachmentBehavior springBehaviour = [[UIAttachmentBehavior alloc] initWithItem:item attachedToAnchor:center];

				springBehaviour.Length = 1.0f;
				springBehaviour.Damping = 0.8f;
				springBehaviour.Frequency = 1.0f;

        // If our touchLocation is not (0,0), we'll need to adjust our item's center "in flight"
				if (!PointF.Empty !=touchLocation) {
					if (ScrollDirection == UICollectionViewScrollDirection.Vertical) {
						Single distanceFromTouch = touchLocation.Y - springBehaviour.AnchorPoint.Y;
                
						Single scrollResistance;
						if (scrollResistanceFactor != null) {
							scrollResistance = distanceFromTouch / scrollResistanceFactor;
						} else {
							scrollResistance = distanceFromTouch / kScrollResistanceFactorDefault;
						}
                
						if (_latestDelta < 0) {
							center.Y += Math.Max (_latestDelta, _latestDelta * scrollResistance);
						} else {
							center.Y += Math.Min (_latestDelta, _latestDelta * scrollResistance);
						}
                
						item.Center = center;
                
					} else {
						Single distanceFromTouch =  touchLocation.X - springBehaviour.AnchorPoint.X;
                
						Single scrollResistance;
						if (scrollResistanceFactor) {
							scrollResistance = distanceFromTouch / scrollResistanceFactor;
						} else {
							scrollResistance = distanceFromTouch / kScrollResistanceFactorDefault;
						}
						if (_latestDelta < 0) {
							center.X += Math.Max (_latestDelta, _latestDelta * scrollResistance);
						} else {
							center.X += Math.Min (_latestDelta, _latestDelta * scrollResistance);
						}
						item.Center = center;
					}
				}
        

				var snapBehavior = new UISnapBehavior(item,);
				_dynamicAnimator.AddBehavior (snapBehavior);
//        [self.dynamicAnimator addBehavior:springBehaviour];
				if(item.RepresentedElementCategory == UICollectionElementCategory.Cell)
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

        public override bool ShouldInvalidateLayoutForBoundsChange(RectangleF newBounds)
        {
			UIScrollView scrollView = CollectionView;

			Single delta;
			if (ScrollDirection == UICollectionViewScrollDirection.Vertical) {
				delta = newBounds.Y - scrollView.Bounds.Y;
			} else {
				delta = newBounds.X - scrollView.Bounds.X;
			}
			_latestDelta = delta;

			PointF touchLocation = CollectionView.PanGestureRecognizer.LocationInView;// locationInView:self.collectionView];

			for (int i = 0; i < _dynamicAnimator.Behaviors.Length; i++) {
				var item = _dynamicAnimator.Behaviors [i];

				UIAttachmentBehavior springBehaviour = new UIAttachmentBehavior (item, center);

				//[_dynamicAnimator.behaviors enumerateObjectsUsingBlock:^(UIAttachmentBehavior *springBehaviour, NSUInteger idx, BOOL *stop) {
				if (ScrollDirection == UICollectionViewScrollDirection.Vertical) {
					Single distanceFromTouch = touchLocation.Y - springBehaviour.AnchorPoint.Y);

					Single scrollResistance;
					if (scrollResistanceFactor){ 
						scrollResistance = distanceFromTouch / scrollResistanceFactor;
					}else{ 
						scrollResistance = distanceFromTouch / kScrollResistanceFactorDefault;
					}
					UICollectionViewLayoutAttributes item = springBehaviour.Items.FirstOrDefault();
					PointF center = item.Center;
					if (delta < 0)
					{center.Y += Math.Max(delta, delta*scrollResistance);}
					else
					{center.Y += Math.Min(delta, delta*scrollResistance);
					}

					item.Center = center;

					_dynamicAnimator.UpdateItemUsingCurrentState(item);
				} else {
					Single distanceFromTouch = touchLocation.X - springBehaviour.AnchorPoint.X;

					Single scrollResistance;
					if (scrollResistanceFactor)
					{scrollResistance = distanceFromTouch / scrollResistanceFactor;
					}
					else
					{scrollResistance = distanceFromTouch / kScrollResistanceFactorDefault;
					}
					UICollectionViewLayoutAttributes item = springBehaviour.Items.FirstOrDefault();
					PointF center = item.Center;
					if (delta < 0)
					{center.X += Math.Max(delta, delta*scrollResistance);
					}
					else
					{center.X += Math.Min(delta, delta*scrollResistance);}

					item.Center = center;

					_dynamicAnimator.UpdateItemUsingCurrentState(item);
				}
			}

			return false;
		}

		public override UICollectionViewLayoutAttributes LayoutAttributesForItem (NSIndexPath indexPath)
		{
			UICollectionViewLayoutAttributes dynamicLayoutAttributes = _dynamicAnimator.GetLayoutAttributesForCell (indexPath);
			// Check if dynamic animator has layout attributes for a layout, otherwise use the flow layouts properties. This will prevent crashing when you add items later in a performBatchUpdates block (e.g. triggered by NSFetchedResultsController update)
			return (dynamicLayoutAttributes != null) ? dynamicLayoutAttributes : base.LayoutAttributesForItem (indexPath);
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