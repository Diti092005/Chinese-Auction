import { Component } from '@angular/core';
import { ButtonModule } from 'primeng/button';

@Component({
    selector: 'primeng-demo',
    templateUrl: './primeng-demo.html',
    standalone: true,
    imports: [ButtonModule]
})
export class PrimengDemo {}