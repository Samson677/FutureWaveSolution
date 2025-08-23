using FutureWave.Models.Dtos;
using Microsoft.AspNetCore.Components;

namespace FutureWave.Web.Pages
{
    public class ContactBase: ComponentBase
    {
        public ContactDto contactDto = new();
       

        public bool isSucess = false;
  



        public async Task SendAsync()
        {
            try
            {

                

                isSucess = true;

              
            }
            catch (Exception)
            {

                isSucess = false;
            }
            
        }
    }
}
