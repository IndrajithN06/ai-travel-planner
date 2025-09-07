import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ItineraryViewComponent } from './itinerary-view.component';

describe('ItineraryViewComponent', () => {
  let component: ItineraryViewComponent;
  let fixture: ComponentFixture<ItineraryViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ItineraryViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ItineraryViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
