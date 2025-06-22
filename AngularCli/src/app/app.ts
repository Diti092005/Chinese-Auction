import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { PrimengDemo } from './primeng-demo/primeng-demo';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected title = 'base-app';
}
