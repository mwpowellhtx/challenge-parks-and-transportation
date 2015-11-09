#pragma once

#include <iostream>
#include <string>
#include <vector>

namespace challenges {

    class challenge_base {

        std::ostream * _pos;

    protected:

        virtual std::vector<std::string> read_lines(std::istream & is) {
            std::vector<std::string> lines;
            while (!is.eof()) {
                std::string line;
                std::getline(is, line);
                lines.push_back(line);
            }
            return lines;
        }

        void init(std::istream & is) {
            read_lines(is);
        }

    protected:

        challenge_base(std::ostream * pos)
            : _pos(pos) {
        }

    protected:

        virtual void run() = 0;

        virtual void report(std::ostream & os) = 0;

        void finish() {
            run();
            report(*_pos);
        }

    public:

        virtual ~challenge_base() {
        }
    };
}
