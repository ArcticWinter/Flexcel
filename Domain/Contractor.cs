using System.Collections.Generic;
using System.Linq;
using LINQtoCSV;
namespace Domain
{
    public class Contractor
    {
        private int type2;
        private int type3;
        private int type5;
        private int type6;
        private int type7;

        public List<Offer> winningOffers;
        public List<Offer> ineligibleOffers;

        public string ReferenceNumberBasicInformationPDF { get; set; }
        public string UserID { get; set; }
        public string CompanyName { get; set; }
        public string ManagerName { get; set; }  
        public int NumberOfType2PledgedVehicles { get; set; }
        public int NumberOfType3PledgedVehicles { get; set; }
        public int NumberOfType5PledgedVehicles { get; set; }
        public int NumberOfType6PledgedVehicles { get; set; }
        public int NumberOfType7PledgedVehicles { get; set; }
        public string TryParseValueType2PledgedVehicles { get; set; }
        public string TryParseValueType3PledgedVehicles { get; set; }
        public string TryParseValueType5PledgedVehicles { get; set; }
        public string TryParseValueType6PledgedVehicles { get; set; }
        public string TryParseValueType7PledgedVehicles { get; set; }
        public int NumberOfWonType2Offers { get; private set; }
        public int NumberOfWonType3Offers { get; private set; }
        public int NumberOfWonType5Offers { get; private set; }
        public int NumberOfWonType6Offers { get; private set; }
        public int NumberOfWonType7Offers { get; private set; }

        public Contractor()
        {
            winningOffers = new List<Offer>();
            ineligibleOffers = new List<Offer>();
        }
        public Contractor(
            string referenceNumberBasicInformationPDF, string userID, string companyName,
            string managerName, int numberOfType2PledgedVehicles, int numberOfType3PledgedVehicles,int numberOfType5PledgedVehicles,
            int numberOfType6PledgedVehicles, int numberOfType7PledgedVehicles) : this()
        {
            this.ReferenceNumberBasicInformationPDF = referenceNumberBasicInformationPDF;
            this.UserID = userID;
            this.CompanyName = companyName;
            this.ManagerName = managerName;
            this.NumberOfType2PledgedVehicles = numberOfType2PledgedVehicles;
            this.NumberOfType3PledgedVehicles = numberOfType3PledgedVehicles;
            this.NumberOfType5PledgedVehicles = numberOfType5PledgedVehicles;
            this.NumberOfType6PledgedVehicles = numberOfType6PledgedVehicles;
            this.NumberOfType7PledgedVehicles = numberOfType7PledgedVehicles;
        }

        public void AddWonOffer(Offer offer)
        {
            bool alreadyOnTheList = winningOffers.Any(item => item.OfferReferenceNumber == offer.OfferReferenceNumber);
            if (!alreadyOnTheList)
            {
                winningOffers.Add(offer);
            }
            else 
            {
                foreach(Offer winOffer in winningOffers)
                {
                    if (winOffer.OfferReferenceNumber == offer.OfferReferenceNumber)
                    {
                        winOffer.IsEligible = true; 
                    }
                }
                    
            }
         
        }
        public List<Offer> ReturnIneligibleOffers()
        {
            List<Offer> InEligibleOffersToReturn = new List<Offer>();
            foreach (Offer offer in winningOffers)
            {
                if (!offer.IsEligible)
                {
                    InEligibleOffersToReturn.Add(offer);
                }
            }
            return InEligibleOffersToReturn;
        }
        public void RemoveIneligibleOffersFromWinningOffers()
        {
            List<Offer> toBeRemoved = new List<Offer>();
            foreach (Offer offer in winningOffers)
            {
                if (!offer.IsEligible)
                {
                    ineligibleOffers.Add(offer);
                    toBeRemoved.Add(offer);
                }
            }
            if (toBeRemoved.Count > 0)
            {
                foreach (Offer offer in toBeRemoved)
                {
                    winningOffers.Remove(offer);
                }
            }
        }         
        public List<Offer> CompareNumberOfWonOffersAgainstVehicles()
        {
            List<Offer> offersWithConflict = new List<Offer>();
            type2 = 0;
            type3 = 0;
            type5 = 0;
            type6 = 0;
            type7 = 0;
            if (winningOffers.Count > 0)
            {
                foreach (Offer offer in winningOffers)
                {
                    if (offer.IsEligible)
                    {
                        if (offer.RequiredVehicleType == 2)
                        {
                            type2++;
                        }
                        if (offer.RequiredVehicleType == 3)
                        {
                            type3++;
                        }
                        if (offer.RequiredVehicleType == 5)
                        {
                            type5++;
                        }
                        if (offer.RequiredVehicleType == 6)
                        {
                            type6++;
                        }
                        if (offer.RequiredVehicleType == 7)
                        {
                            type7++;
                        }
                    }
                }
            }
            if (winningOffers.Count > 0)
            {
                if (NumberOfType2PledgedVehicles == 0 && NumberOfType3PledgedVehicles==0 && NumberOfType5PledgedVehicles == 0 && NumberOfType6PledgedVehicles == 0 && NumberOfType7PledgedVehicles == 0)
                {
                    //If all pledged vehicles is 0, it means they have unlimited amount of vehicles available
                }
                else
                {
                    foreach (Offer ofr in IfTooManyWonOffers(NumberOfType2PledgedVehicles, type2, 2))
                    {
                        offersWithConflict.Add(ofr);
                    }
                    foreach (Offer ofr in IfTooManyWonOffers(NumberOfType3PledgedVehicles, type3, 3))
                    {
                        offersWithConflict.Add(ofr);
                    }
                    foreach (Offer ofr in IfTooManyWonOffers(NumberOfType5PledgedVehicles, type5, 5))
                    {
                        offersWithConflict.Add(ofr);
                    }
                    foreach (Offer ofr in IfTooManyWonOffers(NumberOfType6PledgedVehicles, type6, 6))
                    {
                        offersWithConflict.Add(ofr);
                    }
                    foreach (Offer ofr in IfTooManyWonOffers(NumberOfType7PledgedVehicles, type7, 7))
                    {
                        offersWithConflict.Add(ofr);
                    }
                }
            }

            return offersWithConflict;
        }
        private List<Offer> IfTooManyWonOffers(int numberOfPledgedVehicles, int numberOfWonOffersWithThisType, int type)
        {
            List<Offer> offersToCheck = new List<Offer>();
            List<Offer> listOfOffersToReturn = new List<Offer>();
            foreach (Offer winningOffer in winningOffers)
            {
                if (winningOffer.IsEligible && winningOffer.RequiredVehicleType == type)
                {
                    offersToCheck.Add(winningOffer);
                }
            }


            if (numberOfPledgedVehicles < numberOfWonOffersWithThisType)
            {
                if (numberOfPledgedVehicles == 0) //This is done because, sometimes contractors place bids on routenumbers, they don't have the correct vehicle type for. 
                {
                    foreach (Offer offer in winningOffers)
                    {
                        if (offer.RequiredVehicleType == type)
                        {
                            offer.IsEligible = false;
                        }
                    }
                }
                else
                {
                    listOfOffersToReturn = FindOptimalWins(offersToCheck, numberOfPledgedVehicles);
                }
            }

            return listOfOffersToReturn;
        }
        private List<Offer> FindOptimalWins(List<Offer> offersToCheck, int numberOfPledgedVehicles)
        {
            List<Offer> offersWithConflict = new List<Offer>();
            List<Offer> offersToChooseFrom = offersToCheck.OrderByDescending(x => x.DifferenceToNextOffer).ThenBy(x => x.ContractorPriority).ToList();

            foreach (Offer offer in offersToChooseFrom)
            {
                if (offer.DifferenceToNextOffer >= offersToChooseFrom[numberOfPledgedVehicles - 1].DifferenceToNextOffer)
                {
                    offer.IsEligible = true;
                }
                else
                {
                    offer.IsEligible = false;
                }
            }
          
            int eligibleOffers = 0;
            foreach (Offer offer in offersToChooseFrom)
            {
                if (offer.IsEligible)
                {
                    eligibleOffers++;
                }
            }
            if (eligibleOffers > numberOfPledgedVehicles)
            {              
                if (offersToChooseFrom[numberOfPledgedVehicles - 1].ContractorPriority != offersToChooseFrom[numberOfPledgedVehicles].ContractorPriority)
                {
                    int length = offersToCheck.Count;
              
                    for (int i = numberOfPledgedVehicles; i < length; i++)
                    {
                        if (offersToChooseFrom[i].DifferenceToNextOffer == offersToChooseFrom[numberOfPledgedVehicles - 1].DifferenceToNextOffer)
                        {
                            offersToChooseFrom[i].IsEligible = false;
                        }
                    }
                }
                else
                {
                    foreach (Offer offer in offersToChooseFrom)
                    {
                        if (offer.DifferenceToNextOffer == offersToChooseFrom[numberOfPledgedVehicles-1].DifferenceToNextOffer && offer.IsEligible)
                        {
                            offersWithConflict.Add(offer);
                        }
                    }
                    if (offersWithConflict.Count == 1)
                    {
                        offersWithConflict.Clear();
                    }
                }
                              
            }
            return offersWithConflict;
        }
        public void CountNumberOfWonOffersOfEachType(List<Offer> outPutList)
        {
            NumberOfWonType2Offers = 0;
            NumberOfWonType3Offers = 0;
            NumberOfWonType5Offers = 0;
            NumberOfWonType6Offers = 0;
            NumberOfWonType7Offers = 0;


            foreach (Offer offer in outPutList)
            {
                if (offer.UserID == UserID)
                {
                    if (offer.RequiredVehicleType == 2)
                    {
                        NumberOfWonType2Offers++;
                    }
                    if (offer.RequiredVehicleType == 3)
                    {
                        NumberOfWonType3Offers++;
                    }
                    if (offer.RequiredVehicleType == 5)
                    {
                        NumberOfWonType5Offers++;
                    }
                    if (offer.RequiredVehicleType == 6)
                    {
                        NumberOfWonType6Offers++;
                    }
                    if (offer.RequiredVehicleType == 7)
                    {
                        NumberOfWonType7Offers++;
                    }

                }
            }

        }
    }
}