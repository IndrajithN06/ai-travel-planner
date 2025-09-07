import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlanTripComponent } from './plan-trip.component';

describe('PlanTripComponent', () => {
  let component: PlanTripComponent;
  let fixture: ComponentFixture<PlanTripComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PlanTripComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlanTripComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
