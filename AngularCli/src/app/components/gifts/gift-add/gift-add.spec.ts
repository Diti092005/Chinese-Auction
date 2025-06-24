import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GiftAdd } from './gift-add';

describe('GiftAdd', () => {
  let component: GiftAdd;
  let fixture: ComponentFixture<GiftAdd>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GiftAdd]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GiftAdd);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
