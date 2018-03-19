using System.Collections.Generic;

namespace Domain
{
    public sealed class ListContainer
    {
        public List<RouteNumber> routeNumberList;
        public List<Contractor> contractorList;
        public List<Offer> outputList;
        public List<Offer> conflictList;
        static readonly ListContainer listContainer = new ListContainer();

        private ListContainer()
        {
            routeNumberList = new List<RouteNumber>(); 
            contractorList = new List<Contractor>();
            outputList = new List<Offer>();
            conflictList = new List<Offer>();
        }

        public static ListContainer GetInstance()
        {
          return listContainer;
        }
        public void GetLists(List<RouteNumber> routeNumberList, List<Contractor> contractorList)
        {
            this.routeNumberList = routeNumberList;
            this.contractorList = contractorList;
        }
    }
}
