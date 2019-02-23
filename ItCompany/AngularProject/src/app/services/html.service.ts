import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class HtmlService {

  constructor() { }

  public static  strip(html) {
    var tmp = document.createElement("DIV");
    tmp.innerHTML = this.encodeNewLines(html, "[rn]");
    let htmlWithoutTags = tmp.textContent || tmp.innerText || "";
    return this.decodeNewLines(htmlWithoutTags, "[rn]");
  }

  public static encodeNewLines(html, newLineCode){
    return html.
      split("<div>").join(newLineCode).
      split("</div>").join("").
      split("<br>").join(newLineCode).
      split("<p>").join(newLineCode).
      split("</p>").join("");
  }

  public static decodeNewLines(html, newLineCode){
    return html.split(newLineCode).join("<br>");
  }
}
