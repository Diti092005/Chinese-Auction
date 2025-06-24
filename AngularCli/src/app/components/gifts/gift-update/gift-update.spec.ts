import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GiftUpdate } from './gift-update';

describe('GiftUpdate', () => {
  let component: GiftUpdate;
  let fixture: ComponentFixture<GiftUpdate>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GiftUpdate]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GiftUpdate);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
