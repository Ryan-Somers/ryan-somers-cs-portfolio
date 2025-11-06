import { Link } from "@nextui-org/link";
import { Snippet } from "@nextui-org/snippet";
import { Code } from "@nextui-org/code";
import { button as buttonStyles } from "@nextui-org/theme";

import { siteConfig } from "@/config/site";
import { title, subtitle } from "@/components/primitives";
import DefaultLayout from "@/layouts/default";

export default function IndexPage() {
  return (
    <DefaultLayout>
      <section className="flex flex-col items-center justify-center gap-4 py-8 md:py-10">
        <div className="inline-block max-w-lg text-center justify-center">
          <span className={title()}>Beautiful&nbsp;</span>
          <span className={title({ color: "violet" })}>Products&nbsp;</span>
          <br />
          <span className={title()}>
            at Ryan's Store
          </span>
          <div className={subtitle({ class: "mt-4" })}>
            Manage your amazing items with ease.
          </div>
        </div>

        <div className="flex gap-3">
          <Link
            isExternal
            className={buttonStyles({
              color: "primary",
              radius: "full",
              variant: "shadow",
            })}
            href={siteConfig.links.docs}
          >
            Products
          </Link>
          <Link
              isExternal
              className={buttonStyles({
                color: "secondary",
                radius: "full",
                variant: "shadow",
              })}
              href={siteConfig.links.reports}
          >
            Reports
          </Link>
        </div>

        <div className="mt-8">
          <Snippet hideCopyButton hideSymbol variant="flat">
            <span>
              Website built in NextUI & React by{" "}
              <Code color="primary">Ryan Somers</Code>
            </span>
          </Snippet>
        </div>
      </section>
    </DefaultLayout>
  );
}
