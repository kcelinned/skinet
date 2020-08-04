import { Component, OnInit } from '@angular/core';
import { BasketService } from './basket/basket.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  title = 'Skinet';

  constructor(private basketService: BasketService) { }

  ngOnInit(): void {

    // when the app is initialised -
    // check if there is a basketId in local storage 
    const basketId = localStorage.getItem('basket_id');
    if (basketId){
      // get this basket from api and subscribe to it 
      this.basketService.getBasket(basketId).subscribe(() => {
        console.log('initalised basket');
      }, error =>{
        console.log(error);
      });
    }
  }
}
