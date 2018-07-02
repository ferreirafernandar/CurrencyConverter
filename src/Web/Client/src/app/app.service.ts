import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';


@Injectable()
export class AppService {

  constructor(private http: HttpClient) {

  }

  getCurrencies(): Observable<Object> {
    return this.http
      .get('http://localhost:53739/api/converter/currencies');
  }

  converterCurrency(from, to, amount): Observable<Object> {
    return this.http
      .get(`http://localhost:53739/api/converter/currencyconversion?currencyFrom=${from}&currencyTo=${to}&amount=${amount}`);
  }

}
