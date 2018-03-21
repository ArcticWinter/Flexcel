using System.Collections.Generic;
using System.Linq;
using Domain;

namespace Logic
{
    public class SelectionController
    {       
        public List<RouteNumber> RouteNumberList;
        public Selection Selection;
        public List<RouteNumber> SortedRouteNumberList;
        ListContainer ListContainer = ListContainer.GetInstance();
        
        public SelectionController()
        {
            RouteNumberList = new List<RouteNumber>();
            Selection = new Selection();
        }

        private void SortRouteNumberList(List<RouteNumber> RouteNumberList)
        {
            SortedRouteNumberList = RouteNumberList.OrderBy(x => x.RouteID).ToList();
            foreach (RouteNumber routeNumber in SortedRouteNumberList)
            {
                routeNumber.offers = routeNumber.offers.OrderBy(x => x.OperationPrice).ThenBy(x => x.RouteNumberPriority).ToList();
            }
        }
        public void SelectWinners()
        {
            RouteNumberList = ListContainer.routeNumberList;
            SortRouteNumberList(RouteNumberList);
            List<Offer> offersToAssign = new List<Offer>();

            Selection.CalculateOperationPriceDifferenceForOffers(SortedRouteNumberList);
            int lengthOfSortedRouteNumberList = SortedRouteNumberList.Count();
            for (int i = 0; i < lengthOfSortedRouteNumberList; i++)
            {
                List<Offer> toAddToAssign = Selection.FindWinner(SortedRouteNumberList[i]);
                foreach (Offer offer in toAddToAssign)
                {
                    offersToAssign.Add(offer);
                }
            }
            List<Offer> offersThatAreIneligible = Selection.AssignWinners(offersToAssign, SortedRouteNumberList);

            if (DoAllRouteNumbersHaveWinner(offersThatAreIneligible))
            {
                Selection.CheckIfContractorHasWonTooManyRouteNumbers(CreateWinnerList(), SortedRouteNumberList);
                Selection.CheckForMultipleWinnersForEachRouteNumber(CreateWinnerList());
                List<Offer> winningOffers = CreateWinnerList();
                foreach (Offer offer in winningOffers)
                {
                    ListContainer.outputList.Add(offer);
                }
            }
            else
            {
                ContinueUntilAllRouteNumbersHaveWinner(offersThatAreIneligible);
            }
        }
        private void ContinueUntilAllRouteNumbersHaveWinner(List<Offer> offersThatAreIneligible)
        {
            List<Offer> offersThatHaveBeenMarkedIneligible = offersThatAreIneligible;
            List<Offer> offersToAssign = new List<Offer>();

            foreach (Offer offer in offersThatHaveBeenMarkedIneligible)
            {
                foreach (RouteNumber routeNumber in SortedRouteNumberList)
                {
                    if (routeNumber.RouteID == offer.RouteID)
                    {
                        List<Offer> offersToAssignToContractor = Selection.FindWinner(routeNumber);
                        foreach (Offer ofr in offersToAssignToContractor)
                        {
                            offersToAssign.Add(ofr);
                        }
                    }
                }
            }
            offersThatHaveBeenMarkedIneligible = Selection.AssignWinners(offersToAssign, SortedRouteNumberList);
            bool allRouteNumberHaveWinner = DoAllRouteNumbersHaveWinner(offersThatHaveBeenMarkedIneligible);
            if (allRouteNumberHaveWinner)
            {
                Selection.CheckIfContractorHasWonTooManyRouteNumbers(CreateWinnerList(), SortedRouteNumberList);
                Selection.CheckForMultipleWinnersForEachRouteNumber(CreateWinnerList());
                foreach (Offer offer in CreateWinnerList())
                {
                    ListContainer.outputList.Add(offer);
                }
            } // Sidste punkt
            else
            {                
                ContinueUntilAllRouteNumbersHaveWinner(offersThatHaveBeenMarkedIneligible);
            }
        }
        public List<Offer> CreateWinnerList()
        {
            List<Offer> winningOffers = new List<Offer>();

            foreach (Contractor contractor in ListContainer.contractorList)
            {
                foreach (Offer offer in contractor.winningOffers)
                {
                    winningOffers.Add(offer);
                }
            }
            return winningOffers;
        }
        private bool DoAllRouteNumbersHaveWinner(List<Offer> offersThatAreIneligible)
        {
            if (offersThatAreIneligible.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

