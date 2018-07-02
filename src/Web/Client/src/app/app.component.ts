import { Component } from '@angular/core';
import { AppService } from './app.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'app';
  currencies: Array<any> = [];
  amount: number;
  currencyFrom = 'Selecione...';
  currencyTo = 'Selecione...';
  result: any;

  constructor(private appService: AppService) {
    this.obterMoedas();
  }

  obterMoedas() {
    this.appService.getCurrencies()
      .subscribe((response: any) => {
        console.log('getCurrencies ', response);

        Object.keys(response).map(currency => {
          const temp = { code: currency, name: response[currency] };
          this.currencies.push(temp);
        });
      }, error => {
        console.log('error ', error);
      });
  }

  converter() {
    this.appService.converterCurrency(this.currencyFrom, this.currencyTo, this.amount)
      .subscribe(response => {
        console.log('converteu ', response);
        this.result = response;
      }, error => {
        console.log('error ', error);
      });
  }
}
