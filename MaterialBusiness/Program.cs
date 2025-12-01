using MaterialBusiness;
using System;
using System.Linq;

class Program
{
    static Business business;
    static List<Fabric> items = new List<Fabric>();
    static void Main(string[] args)
    {

        business = new Business("Fabric emporium", "123 jasmine lane");
        items = business.Items.GetAllFabrics();


    }
}