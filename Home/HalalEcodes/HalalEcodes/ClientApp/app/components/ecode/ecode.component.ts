import { Component } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { EcodeService } from '../../services/ecode.service';
import { Ecode } from '../../models/ecode';

@Component({
    selector: 'ecode',
    templateUrl: './ecode.component.html'
})

export class EcodeComponent {
    public ecode: Ecode;

    constructor(private ecodeService: EcodeService, private route: ActivatedRoute) { }

    ngOnInit() {
        this.route.params.subscribe(params => {
            console.log(params['code']);
            this.ecodeService.getEcode(params['code']).subscribe(ecode => {
                this.ecode = ecode;
            });
        });
    }
}