import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { PrimengDemo } from './primeng-demo/primeng-demo';
import { CreditPaymentComponent } from "./components/credit-payment-component/credit-payment-component";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CreditPaymentComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected title = 'base-app';
}
